using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapSystem: MonoBehaviour
{
    private MapData mapData;

    private Dictionary<string, Tile> tiles;
    private Dictionary<string, Tilemap> tilemaps;

    private Vector2Int selectedCell;


    void Awake()
    {
        InitData();

        InitTiles();
        InitTilemaps();

        SetupMap();

        ConstructMap();
    }


    private void InitData()
    {
        selectedCell = new Vector2Int();

        mapData = new MapData
        {
            showCollision = false,
            cells = new CellData[(int)Mathf.Pow(MapInfo.MapWidth, 2)],
            rooms = new List<RoomData>(MapInfo.NumberOfSeedRooms),
            placeholders = new List<RectInt>()
        };

        for (int x = -MapInfo.MapSize; x <= MapInfo.MapSize; x++)
        {
            for (int y = -MapInfo.MapSize; y <= MapInfo.MapSize; y++)
            {
                CellData cellData = mapData.cells[MapUtil.CoordsToIndex(x, y)];
                cellData.position = new Vector2Int(x, y);

                mapData.cells[MapUtil.CoordsToIndex(x, y)] = cellData;
            }
        }
    }


    private void InitTiles()
    {
        tiles = new Dictionary<string, Tile>();

        foreach (Tile tile in Resources.LoadAll<Tile>("Tiles"))
        {
            tiles[tile.name] = tile;
        }
    }


    private void InitTilemaps()
    {
        tilemaps = new Dictionary<string, Tilemap>();

        Tilemap[] tilemapsArray = GameObject
            .Find("Map").GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }

        TilemapRenderer collisionRenderer = tilemaps["Collision"]
            .GetComponent<TilemapRenderer>();

        collisionRenderer.enabled = mapData.showCollision;
    }


    private void SetupMap()
    {
        SetupGroundLayer();
        SetupWallsLayer();
        SetupOverlayLayer();
    }


    private void SetupGroundLayer()
    {
        for (int x = -MapInfo.MapSize; x <= MapInfo.MapSize; x++)
        {
            for (int y = -MapInfo.MapSize; y <= MapInfo.MapSize; y++)
            {
                SetupGround(x, y, GroundType.Grass);
            }
        }

        SetupGround(0, 0, GroundType.None);
    }


    private void SetupWallsLayer()
    {
        SetPlaceholdersForWalls();

        SeedRooms();
        ExpandRooms();
        FinalizeRooms();
    }


    private void SetPlaceholdersForWalls()
    {
        int roadWidth = 8;

        RectInt eastWestRoadPlaceholder = new RectInt(
            -MapInfo.MapSize, roadWidth / 2,
            MapInfo.MapWidth, roadWidth
        );
        RectInt northSouthRoadPlaceholder = new RectInt(
            -roadWidth / 2, -MapInfo.MapSize,
            roadWidth, MapInfo.MapWidth
        );

        mapData.placeholders.Add(eastWestRoadPlaceholder);
        mapData.placeholders.Add(northSouthRoadPlaceholder);

        SetupGround(eastWestRoadPlaceholder, GroundType.Stone);
        SetupGround(northSouthRoadPlaceholder, GroundType.Stone);
    }


    private void SeedRooms()
    {
        for (int i = 0; i < MapInfo.NumberOfSeedRooms; i++)
        {
            RectInt bounds = GetNewRoomLocation(8);

            RoomData roomData = new RoomData
            {
               bounds = bounds,
               groundType = GroundType.Stone,
               wallType = WallType.StoneWall,
            };

            mapData.rooms.Add(roomData);
        }
    }


    private void ExpandRooms()
    {

    }


    private void FinalizeRooms()
    {
        for (int i = 0; i < mapData.rooms.Count; i++)
        {
            RoomData roomData = mapData.rooms[i];
            roomData.entrances = GenerateEntrances(mapData.rooms[i].bounds);

            mapData.rooms[i] = roomData;

            SetupRoom(mapData.rooms[i]);
        }
    }


    private RectInt GetNewRoomLocation(int size = 4)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2Int randomMapPosition = MapUtil.GetRandomMapPosition();
            RectInt roomBounds = new RectInt(randomMapPosition, new Vector2Int(size, size));

            bool collision = false;

            foreach (RoomData roomData in mapData.rooms)
            {
                if (roomBounds.Overlaps(roomData.bounds))
                {
                    collision = true;
                }
            }

            foreach (RectInt placeholder in mapData.placeholders)
            {
                if (roomBounds.Overlaps(placeholder))
                {
                    collision = true;
                }
            }

            if (collision == false)
            {
                return roomBounds;
            }
        }

        return new RectInt(0, 0, 0, 0);
    }


    private List<EntranceData> GenerateEntrances(RectInt bounds, int number = 2)
    {
        List<EntranceData> entrances = new List<EntranceData>(number);

        for (int i = 0; i < number; i++)
        {
            Vector2Int wallPosition = MapUtil.GetRandomBorderPosition(bounds);

            RectInt entranceBounds = new RectInt(wallPosition, new Vector2Int(1, 1));

            if (entranceBounds.x == bounds.xMin || entranceBounds.x == bounds.xMax)
            {
                entranceBounds.yMax = Random.Range(
                    entranceBounds.yMax,
                    entranceBounds.yMax + (bounds.yMax - entranceBounds.yMax)
                );
            }
            else if (entranceBounds.y == bounds.yMin || entranceBounds.y == bounds.yMax)
            {
                entranceBounds.xMax = Random.Range(
                    entranceBounds.xMax,
                    entranceBounds.xMax + (bounds.xMax - entranceBounds.xMax)
                );
            }

            EntranceData entranceData = new EntranceData
            {
                bounds = entranceBounds,
            };

            entrances.Add(entranceData);
        }

        return entrances;
    }


    private void SetupOverlayLayer()
    {

    }


    
    // Setup Methods


    private void SetCellSolid(int x, int y, bool solid = true)
    {
        SetCellSolid(new Vector2Int(x, y), solid);
    }


    private void SetCellSolid(Vector2Int position, bool solid = true)
    {
        if (MapUtil.OnMap(position))
        {
            mapData.cells[MapUtil.CoordsToIndex(position)].solid = solid;
        }
    }


    private void SetupGround(int x, int y, GroundType groundType)
    {
        SetupGround(new Vector2Int(x, y), groundType);
    }


    private void SetupGround(Vector2Int position, GroundType groundType)
    {
        if (MapUtil.OnMap(position))
        {
            mapData.cells[MapUtil.CoordsToIndex(position)].groundType = groundType;
        }
    }


    private void SetupGround(RectInt bounds, GroundType groundType, bool fill = true)
    {
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                if (fill || MapUtil.OnRectBoundary(x, y, bounds))
                {
                    SetupGround(x, y, groundType);
                }
            }
        }
    }


    private void SetupWall(int x, int y, WallType wallType) {
        SetupWall(new Vector2Int(x, y), wallType);
    }


    private void SetupWall(Vector2Int position, WallType wallType)
    {
        if (MapUtil.OnMap(position))
        {
            int cellIndex = MapUtil.CoordsToIndex(position);

            mapData.cells[cellIndex].solid = true;
            mapData.cells[cellIndex].wallType = wallType;
        }
    }

    
    private void SetupOverlay(int x, int y, OverlayType overlayType)
    {
        SetupOverlay(new Vector2Int(x, y), overlayType);
    }


    private void SetupOverlay(Vector2Int position, OverlayType overlayType)
    {
        if (MapUtil.OnMap(position))
        {
            mapData.cells[MapUtil.CoordsToIndex(position)].overlayType = overlayType;
        }
    }



    // Higher Level Setup Methods

    private void SetupRoom(RoomData roomData)
    {
        for (int x = roomData.bounds.xMin; x <= roomData.bounds.xMax; x++)
        {
            for (int y = roomData.bounds.yMin; y <= roomData.bounds.yMax; y++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y);

                SetupGround(cellPosition, roomData.groundType);

                if (MapUtil.EntranceExistsAt(x, y, roomData) == false)
                {
                    if (roomData.fill || MapUtil.OnRectBoundary(x, y, roomData.bounds))
                    {
                        SetupWall(cellPosition, roomData.wallType);
                    }
                }

                SetupOverlay(cellPosition, roomData.overlayType);
            }
        }
    }



    // Construction Methods

    private void ConstructMap()
    {
        for (int i = 0; i < mapData.cells.Length; i++)
        {
            CellData cellData = mapData.cells[i];
            Vector2Int position = MapUtil.IndexToCoords(i);

            if (cellData.solid)
            {
                ConstructSolid(position);
            }

            if (cellData.groundType != GroundType.None)
            {
                ConstructGround(position, cellData.groundType);
            }

            if (cellData.wallType != WallType.None)
            {
                ConstructWall(position, cellData.wallType);
            }

            if (cellData.overlayType != OverlayType.None)
            {
                ConstructOverlay(position, cellData.overlayType);
            }
        }
    }


    private void ConstructSolid(Vector2Int position)
    {
        tilemaps["Collision"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.overlayTileNames[OverlayType.Collision]]
        );
    }


    private void ConstructGround(Vector2Int position, GroundType cellType)
    {
        tilemaps["Ground"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.groundTileNames[cellType]]
        );
    }


    private void ConstructWall(Vector2Int position, WallType wallType)
    {
        tilemaps["Walls"].SetTile(
            new Vector3Int(position.x, position.y, 3),
            tiles[TileInfo.wallTileNames[wallType]]
        );
    }


    private void ConstructOverlay(Vector2Int position, OverlayType overlayType)
    {
        tilemaps["Overlay"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.overlayTileNames[overlayType]]
        );
    }



    // Helper Functions

    public CellData GetCellData(int x, int y)
    {
        return mapData.cells[MapUtil.CoordsToIndex(x, y)];
    }


    public void SelectCell(Vector2Int position)
    {
        selectedCell = position;

        var selectedPosition = new Vector2Int(position.x, position.y);

        ConstructOverlay(selectedPosition, OverlayType.Selection);
    }


    public void ClearSelection()
    {
        tilemaps["Overlay"].SetTile(new Vector3Int(selectedCell.x, selectedCell.y, 0), null);
    }
}
