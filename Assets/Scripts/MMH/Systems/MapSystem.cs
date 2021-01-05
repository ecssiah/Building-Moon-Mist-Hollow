using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem: MonoBehaviour
{
    private MapData mapData;

    private Vector2Int selectedCell;

    private Dictionary<string, Tile> tiles;
    private Dictionary<string, Tilemap> tilemaps;

    private MapFactory mapFactory;


    void Awake()
    {
        InitData();

        InitTiles();
        InitTilemaps();

        mapData = mapFactory.BuildMap(mapData);

        ConstructBoundary();
        ConstructMap();
    }


    private void InitData()
    {
        mapFactory = gameObject.AddComponent<MapFactory>();

        selectedCell = new Vector2Int();

        mapData = new MapData
        {
            Size = MapInfo.Size,
            ShowCollision = MapInfo.ShowCollision,
            Cells = new CellData[MapInfo.Width * MapInfo.Width],
            Rooms = new List<RoomData>(MapInfo.NumberOfSeedRooms),
            Placeholders = new List<RectInt>()
        };

        for (int x = -MapInfo.Size; x <= MapInfo.Size; x++)
        {
            for (int y = -MapInfo.Size; y <= MapInfo.Size; y++)
            {
                CellData cellData = mapData.GetCell(x, y);
                cellData.Position = new Vector2Int(x, y);

                mapData.SetCell(x, y, cellData);
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

        collisionRenderer.enabled = mapData.ShowCollision;
    }



    // Setup Methods

    public void SetPlaceholder(RectInt bounds)
    {
        mapData.Placeholders.Add(bounds);
    }


    public void SetCellSolid(int x, int y, bool solid = true)
    {
        SetCellSolid(new Vector2Int(x, y), solid);
    }


    public void SetCellSolid(Vector2Int position, bool solid = true)
    {
        if (MapUtil.OnMap(position))
        {
            CellData cellData = mapData.GetCell(position);
            cellData.Solid = solid;

            mapData.SetCell(position, cellData);
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
            CellData cellData = mapData.GetCell(position);
            cellData.GroundType = groundType;

            mapData.SetCell(position, cellData);
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
            CellData cellData = mapData.GetCell(position);

            cellData.Solid = true;
            cellData.WallType = wallType;

            mapData.SetCell(position, cellData);
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
            CellData cellData = mapData.GetCell(position);
            cellData.OverlayType = overlayType;

            mapData.SetCell(position, cellData);
        }
    }



    // Construction Methods

    public void ConstructMap()
    {

        foreach (CellData cellData in mapData.Cells)
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



    // Helper Methods

    public MapData GetMapData()
    {
        return mapData;
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
            string jsonText = JsonUtility.ToJson(mapData, true);

            file.Write(jsonText);
        }
    }


    public void LoadMapData(string name)
    {
        using (StreamReader reader = new StreamReader($"Assets/Resources/Data/{name}.json"))
        {
            string jsonText = reader.ReadToEnd();

            mapData = JsonUtility.FromJson<MapData>(jsonText);
        }
    }


}
