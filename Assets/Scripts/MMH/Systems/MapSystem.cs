using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem: MonoBehaviour
{
    private WorldMap worldMap;

    private RoomBuilder roomBuilder;

    private Vector2Int selectedCell;

    private Dictionary<string, Tile> tiles;
    private Dictionary<string, Tilemap> tilemaps;


    void Awake()
    {
        worldMap = gameObject.AddComponent<WorldMap>();

        roomBuilder = gameObject.AddComponent<RoomBuilder>();
        roomBuilder.WorldMap = worldMap;
        
        InitData();

        InitTiles();
        InitTilemaps();
    }


    void Start()
    {
        SetupMap();

        ConstructBoundary();
        ConstructMap();
    }


    private void InitData()
    {
        selectedCell = new Vector2Int();

        for (int x = -MapInfo.Size; x <= MapInfo.Size; x++)
        {
            for (int y = -MapInfo.Size; y <= MapInfo.Size; y++)
            {
                CellData cellData = worldMap.GetCell(x, y);
                cellData.Position = new Vector2Int(x, y);

                worldMap.SetCell(x, y, cellData);
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

        Tilemap[] tilemapsArray = GameObject.Find("Map").GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }

        TilemapRenderer collisionRenderer = tilemaps["Collision"].GetComponent<TilemapRenderer>();

        collisionRenderer.enabled = worldMap.ShowCollision;
    }



    private void SetupMap()
    {
        SetupBase();
        SetupPaths();

        roomBuilder.LayoutRooms();

        SetupRooms();

        SetupTestObstacles();
    }



    // Setup Methods

    public void SetPlaceholder(RectInt bounds)
    {
        worldMap.Placeholders.Add(bounds);
    }


    public void SetCellSolid(int x, int y, bool solid = true)
    {
        SetCellSolid(new Vector2Int(x, y), solid);
    }


    public void SetCellSolid(Vector2Int position, bool solid = true)
    {
        if (MapUtil.OnMap(position))
        {
            CellData cellData = worldMap.GetCell(position);
            cellData.Solid = solid;

            worldMap.SetCell(position, cellData);
        }
    }


    public void SetCellSolid(RectInt bounds, bool solid = true, bool fill = true)
    {
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (fill || MapUtil.OnRectBoundary(x, y, bounds))
                {
                    SetCellSolid(x, y, solid);
                }
            }
        }
    }


    public void SetupGround(int x, int y, GroundType groundType)
    {
        SetupGround(new Vector2Int(x, y), groundType);
    }


    public void SetupGround(Vector2Int position, GroundType groundType)
    {
        if (MapUtil.OnMap(position))
        {
            CellData cellData = worldMap.GetCell(position);
            cellData.GroundType = groundType;

            worldMap.SetCell(position, cellData);
        }
    }


    public void SetupGround(RectInt bounds, GroundType groundType, bool fill = true)
    {
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (fill || MapUtil.OnRectBoundary(x, y, bounds))
                {
                    SetupGround(x, y, groundType);
                }
            }
        }
    }


    public void SetupWall(int x, int y, WallType wallType) {
        SetupWall(new Vector2Int(x, y), wallType);
    }


    public void SetupWall(Vector2Int position, WallType wallType)
    {
        if (MapUtil.OnMap(position))
        {
            CellData cellData = worldMap.GetCell(position);

            cellData.Solid = true;
            cellData.WallType = wallType;

            worldMap.SetCell(position, cellData);
        }
    }


    public void SetupWall(RectInt bounds, WallType wallType, bool fill = false, bool solid = true)
    {
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                if (fill || MapUtil.OnRectBoundary(x, y, bounds))
                {
                    SetupWall(x, y, wallType);

                    if (solid) SetCellSolid(x, y);
                }
            }
        }
    }

    
    public void SetupOverlay(int x, int y, OverlayType overlayType)
    {
        SetupOverlay(new Vector2Int(x, y), overlayType);
    }


    public void SetupOverlay(Vector2Int position, OverlayType overlayType)
    {
        if (MapUtil.OnMap(position))
        {
            CellData cellData = worldMap.GetCell(position);
            cellData.OverlayType = overlayType;

            worldMap.SetCell(position, cellData);
        }
    }


    private void SetupTestObstacles()
    {
        SetCellSolid(2, 0);
        SetCellSolid(2, 2);
        SetCellSolid(0, 2);
        SetCellSolid(-2, 2);
        SetCellSolid(-2, 0);
        SetCellSolid(-2, -2);
        SetCellSolid(0, -2);
        SetCellSolid(2, -2);
    }



    public void Build()
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
    }


    private void SetupPaths()
    {
        RectInt mainEastWest = new RectInt(
            -MapInfo.Size, -MapInfo.PathWidth / 2,
            MapInfo.Width, MapInfo.PathWidth
        );
        RectInt mainNorthSouth = new RectInt(
            -MapInfo.PathWidth / 2, -MapInfo.Size,
            MapInfo.PathWidth, MapInfo.Width
        );

        RectInt north1st = new RectInt(
            -MapInfo.Size, MapInfo.Size / 2 - MapInfo.PathWidth / 2,
            MapInfo.Width, MapInfo.PathWidth / 2
        );
        RectInt south1st = new RectInt(
            -MapInfo.Size, -MapInfo.Size / 2 - MapInfo.PathWidth / 2,
            MapInfo.Width, MapInfo.PathWidth / 2
        );

        RectInt west1st = new RectInt(
            -MapInfo.Size / 2 - MapInfo.PathWidth / 2, -MapInfo.Size,
            MapInfo.PathWidth / 2, MapInfo.Width
        );
        RectInt east1st = new RectInt(
            MapInfo.Size / 2 - MapInfo.PathWidth / 2, -MapInfo.Size,
            MapInfo.PathWidth / 2, MapInfo.Width
        );

        SetupGround(mainEastWest, GroundType.Stone);
        SetupGround(mainNorthSouth, GroundType.Stone);

        SetupGround(north1st, GroundType.Stone);
        SetupGround(south1st, GroundType.Stone);
        SetupGround(west1st, GroundType.Stone);
        SetupGround(east1st, GroundType.Stone);

        SetPlaceholder(mainEastWest);
        SetPlaceholder(mainNorthSouth);

        SetPlaceholder(north1st);
        SetPlaceholder(south1st);
        SetPlaceholder(west1st);
        SetPlaceholder(east1st);
    }


    private void SetupRooms()
    {
        foreach (RoomData roomData in worldMap.Rooms)
        {
            SetupRoom(roomData);
        }
    }


    private void SetupRoom(RoomData roomData)
    {
        for (int x = roomData.Bounds.xMin; x <= roomData.Bounds.xMax; x++)
        {
            for (int y = roomData.Bounds.yMin; y <= roomData.Bounds.yMax; y++)
            {
                Vector2Int cellPosition = new Vector2Int(x, y);

                SetupGround(cellPosition, roomData.GroundType);

                if (MapUtil.EntranceExistsAt(x, y, roomData) == false)
                {
                    if (roomData.Fill || MapUtil.OnRectBoundary(x, y, roomData.Bounds))
                    {
                        SetupWall(cellPosition, roomData.WallType);
                    }
                }

                SetupOverlay(cellPosition, roomData.OverlayType);
            }
        }
    }



    // Construction Methods

    public void ConstructMap()
    {
        foreach (CellData cellData in worldMap.Cells)
        {
            if (cellData.Solid)
            {
                ConstructSolid(cellData.Position);
            }

            if (cellData.GroundType != GroundType.None)
            {
                ConstructGround(cellData.Position, cellData.GroundType);
            }

            if (cellData.WallType != WallType.None)
            {
                ConstructWall(cellData.Position, cellData.WallType);
            }

            if (cellData.OverlayType != OverlayType.None)
            {
                ConstructOverlay(cellData.Position, cellData.OverlayType);
            }
        }
    }


    private void ConstructBoundary()
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



    // Selection Methods

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



    // Saving/Loading - Serialization

    public void SaveMapData(string name)
    {
        using (StreamWriter file = File.CreateText($"Assets/Resources/Data/{name}.json"))
        {
            string jsonCellsText = JsonUtility.ToJson(worldMap, true);

            file.Write(jsonCellsText);
        }
    }


    public void LoadMapData(string name)
    {
        using (StreamReader reader = new StreamReader($"Assets/Resources/Data/{name}.json"))
        {
            string jsonText = reader.ReadToEnd();

            worldMap = JsonUtility.FromJson<WorldMap>(jsonText);
        }
    }
}
