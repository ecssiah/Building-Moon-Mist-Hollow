using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
                SetupCell(x, y, CellType.Grass);
            }
        }

        SetupCell(0, 0, CellType.Water);
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
    }


    private void SetupOverlayLayer()
    {

    }


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


    // Setup Methods

    private void SetupCell(int x, int y, CellType cellType)
    {
        SetupCell(new Vector2Int(x, y), cellType);
    }


    private void SetupCell(Vector2Int position, CellType cellType)
    {
        mapData.cells[MapUtil.CoordsToIndex(position)].cellType = cellType;
    }


    private void SetupBuilding(
        int x, int y, BuildingType buildingType, bool solid = true
    ) {
        SetupBuilding(new Vector2Int(x, y), buildingType, solid);
    }


    private void SetupBuilding(
        Vector2Int position, BuildingType buildingType, bool solid = true
    ) {
        var cellIndex = MapUtil.CoordsToIndex(position);

        mapData.cells[cellIndex].solid = solid;
        mapData.cells[cellIndex].buildingType = buildingType;
    }


    private void SetupOverlay(int x, int y, OverlayType overlayType)
    {
        SetupOverlay(new Vector2Int(x, y), overlayType);
    }


    private void SetupOverlay(Vector2Int position, OverlayType overlayType)
    {
        mapData.cells[MapUtil.CoordsToIndex(position)].overlayType = overlayType;
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

            if (cellData.cellType != CellType.None)
            {
                ConstructCell(position, cellData.cellType);
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


    private void ConstructCell(Vector2Int position, CellType cellType)
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
}
