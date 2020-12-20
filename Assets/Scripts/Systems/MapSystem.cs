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
            cells = new CellData[(int)Mathf.Pow(MapInfo.MapWidth, 2)]
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
            .Find("Map")
            .GetComponentsInChildren<Tilemap>();

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
        SetupBuildingsLayer();
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

        SetupGround(0, 0, GroundType.Water);
        SetCellSolid(0, 0);
    }


    private void SetupBuildingsLayer()
    {
        SetupBuilding(4, 4, BuildingType.StoneWall);
        SetupBuilding(-4, 4, BuildingType.StoneWall);
        SetupBuilding(4, -4, BuildingType.StoneWall);
        SetupBuilding(-4, -4, BuildingType.StoneWall);

        SetupBuilding(-6, 0, BuildingType.StoneWall);
        SetupBuilding(0, 6, BuildingType.StoneWall);
        SetupBuilding(6, 0, BuildingType.StoneWall);
        SetupBuilding(0, -6, BuildingType.StoneWall);


        for (int i = 0; i < 24; i++)
        {
            Vector2Int position = new Vector2Int(
                Random.Range(-MapInfo.MapSize, MapInfo.MapSize),
                Random.Range(-MapInfo.MapSize, MapInfo.MapSize)
            );

            Vector2Int size = new Vector2Int(
                Random.Range(8, 24),
                Random.Range(6, 12)
            );

            EntranceData[] entrances = new EntranceData[2];

            int entrance0XPosition = Random.Range(position.x + 1, position.x + size.x - 1);

            entrances[0] = new EntranceData
            {
                bounds = new RectInt(
                    entrance0XPosition,
                    position.y,
                    Math.Min(4, position.x + size.x - 1 - entrance0XPosition),
                    1
                ),
            };

            int entrance1YPosition = Random.Range(position.y + 1, position.y + size.y - 1);

            entrances[1] = new EntranceData
            {
                bounds = new RectInt(
                   position.x,
                   entrance1YPosition,
                   1,
                   Math.Min(4, position.y + size.y - 1 - entrance1YPosition)
                ),
            };


            RoomData testRoom = new RoomData
            {
                bounds = new RectInt(position, size),
                entrances = entrances,
                groundType = GroundType.Stone,
                buildingType = BuildingType.WoodWall,
            };

            SetupRoom(testRoom);
        }
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


    private void SetupBuilding(int x, int y, BuildingType buildingType) {
        SetupBuilding(new Vector2Int(x, y), buildingType);
    }


    private void SetupBuilding(Vector2Int position, BuildingType buildingType)
    {
        if (MapUtil.OnMap(position))
        {
            int cellIndex = MapUtil.CoordsToIndex(position);

            mapData.cells[cellIndex].solid = true;
            mapData.cells[cellIndex].buildingType = buildingType;
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
                SetupGround(new Vector2Int(x, y), roomData.groundType);

                if (MapUtil.EntranceExistsAt(x, y, roomData) == false)
                {
                    if (roomData.fill)
                    {
                        SetupBuilding(new Vector2Int(x, y), roomData.buildingType);
                    }
                    else
                    {
                        if (MapUtil.OnRectBoundary(x, y, roomData.bounds))
                        {
                            SetupBuilding(new Vector2Int(x, y), roomData.buildingType);
                        }
                    }
                }

                SetupOverlay(new Vector2Int(x, y), roomData.overlayType);
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

            if (cellData.buildingType != BuildingType.None)
            {
                ConstructBuilding(position, cellData.buildingType);
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


    private void ConstructBuilding(Vector2Int position, BuildingType buildingType)
    {
        tilemaps["Buildings"].SetTile(
            new Vector3Int(position.x, position.y, 3),
            tiles[TileInfo.buildingTileNames[buildingType]]
        );
    }


    private void ConstructGround(Vector2Int position, GroundType cellType)
    {
        tilemaps["Ground"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.cellTileNames[cellType]]
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
