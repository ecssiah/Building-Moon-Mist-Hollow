using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem: MonoBehaviour
{
    private MapData mapData;

    private Vector2Int selectedCell;

    private Dictionary<string, Tile> tiles;
    private Dictionary<string, Tilemap> tilemaps;

    private RoomBuilder roomBuilder;


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
            cells = new CellData[(int)Mathf.Pow(MapInfo.Width, 2)],
            rooms = new List<RoomData>(MapInfo.NumberOfSeedRooms),
            placeholders = new List<RectInt>()
        };

        roomBuilder = gameObject.AddComponent<RoomBuilder>();

        for (int x = -MapInfo.Size; x <= MapInfo.Size; x++)
        {
            for (int y = -MapInfo.Size; y <= MapInfo.Size; y++)
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
        SetupBoundaries();
        SetupGroundLayer();
        SetupWallsLayer();
        SetupOverlayLayer();
    }


    private void SetupBoundaries()
    {
        for (int x = MapInfo.WorldBoundary.xMin; x <= MapInfo.WorldBoundary.xMax; x++)
        {
            for (int y = MapInfo.WorldBoundary.yMin; y <= MapInfo.WorldBoundary.yMax; y++)
            {
                if (MapUtil.OnRectBoundary(x, y, MapInfo.WorldBoundary))
                {
                    tilemaps["Collision"].SetTile(
                        new Vector3Int(x, y, 0), tiles["Collision_1"]
                    );
                }
            }
        }
    }


    private void SetupGroundLayer()
    {
        SetupBase();
        SetupPaths();
    }


    private void SetupWallsLayer()
    {
        roomBuilder.Build(ref mapData);

        for (int i = 0; i < mapData.rooms.Count; i++)
        {
            SetupRoom(mapData.rooms[i]);
        }
    }


    private void SetupOverlayLayer()
    {

    }


    private void SetupBase()
    {
        for (int x = -MapInfo.Size; x <= MapInfo.Size; x++)
        {
            for (int y = -MapInfo.Size; y <= MapInfo.Size; y++)
            {
                SetupGround(x, y, GroundType.Grass);
            }
        }

        SetupGround(0, 0, GroundType.Wood);
    }

    
    private void SetupPaths()
    {
        int pathWidth = 8;

        RectInt north1st = new RectInt(
            -MapInfo.Size, MapInfo.Size / 2,
            MapInfo.Width, pathWidth / 2
        );
        RectInt south1st = new RectInt(
            -MapInfo.Size, -MapInfo.Size / 2 - pathWidth / 2,
            MapInfo.Width, pathWidth / 2
        );

        RectInt west1st = new RectInt(
            -MapInfo.Size / 2 - pathWidth / 2, -MapInfo.Size,
            pathWidth / 2, MapInfo.Width
        );
        RectInt east1st = new RectInt(
            MapInfo.Size / 2, -MapInfo.Size,
            pathWidth / 2, MapInfo.Width
        );

        RectInt mainEastWest = new RectInt(
            -MapInfo.Size, -pathWidth / 2,
            MapInfo.Width, pathWidth
        );
        RectInt mainNorthSouth = new RectInt(
            -pathWidth / 2, -MapInfo.Size,
            pathWidth, MapInfo.Width
        );

        SetupGround(north1st, GroundType.Stone);
        SetupGround(south1st, GroundType.Stone);
        SetupGround(west1st, GroundType.Stone);
        SetupGround(east1st, GroundType.Stone);
        SetupGround(mainEastWest, GroundType.Stone);
        SetupGround(mainNorthSouth, GroundType.Stone);

        mapData.placeholders.Add(north1st);
        mapData.placeholders.Add(south1st);
        mapData.placeholders.Add(west1st);
        mapData.placeholders.Add(east1st);
        mapData.placeholders.Add(mainEastWest);
        mapData.placeholders.Add(mainNorthSouth);
    }

    
    
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


    private void SetCellSolid(RectInt bounds, bool solid = true, bool fill = true)
    {
        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                if (fill || MapUtil.OnRectBoundary(x, y, bounds))
                {
                    SetCellSolid(x, y, solid);
                }
            }
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


    public CellData GetCellData(Vector2Int position)
    {
        return mapData.cells[MapUtil.CoordsToIndex(position)];
    }


    public void SelectCell(Vector2Int position)
    {
        selectedCell = position;

        var selectedPosition = new Vector2Int(position.x, position.y);

        ConstructOverlay(selectedPosition, OverlayType.Selection);
    }


    public void ClearSelection()
    {
        tilemaps["Overlay"].SetTile(
            new Vector3Int(selectedCell.x, selectedCell.y, 0), null
        );
    }
}
