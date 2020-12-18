using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : MonoBehaviour
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
        };

        for (int x = -MapInfo.MapSize; x <= MapInfo.MapSize; x++)
        {
            for (int y = -MapInfo.MapSize; y <= MapInfo.MapSize; y++)
            {
                int index = MapUtil.CoordsToIndex(x, y);

                CellData cellData = mapData.cells[index];
                cellData.position = new Vector2Int(x, y);

                mapData.cells[index] = cellData;
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

        tilemaps["Collision"].GetComponent<TilemapRenderer>().enabled = false;
    }



    // Setup Methods

    private void SetupMap()
    {
        SetupGroundMap();
        SetupBuildingsMap();
        SetupOverlayMap();
    }


    private void SetupGroundMap()
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


    private void SetupBuildingsMap()
    {
        SetupBuilding(4, 4, BuildingType.StoneWall);
        SetupBuilding(-4, 4, BuildingType.StoneWall);
        SetupBuilding(4, -4, BuildingType.StoneWall);
        SetupBuilding(-4, -4, BuildingType.StoneWall);

        SetupBuilding(-6, 0, BuildingType.WoodWall);
        SetupBuilding(0, 6, BuildingType.WoodWall);
        SetupBuilding(6, 0, BuildingType.WoodWall);
        SetupBuilding(0, -6, BuildingType.WoodWall);
    }


    private void SetupOverlayMap()
    {

    }


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
        int cellIndex = MapUtil.CoordsToIndex(position);

        CellData cellData = mapData.cells[cellIndex];

        cellData.solid = solid;
        cellData.buildingType = buildingType;

        mapData.cells[cellIndex] = cellData;
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

            if (cellData.solid)
            {
                ConstructSolid(position);
            }
        }
    }


    private void ConstructCell(Vector2Int position, CellType cellType)
    {
        tilemaps["Tiles"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.cellTileNames[cellType]]
        );
    }


    private void ConstructBuilding(Vector2Int position, BuildingType buildingType)
    {
        tilemaps["Buildings"].SetTile(
            new Vector3Int(position.x, position.y, 3),
            tiles[TileInfo.buildingTileNames[buildingType]]
        );
    }


    private void ConstructOverlay(Vector2Int position, OverlayType overlayType)
    {
        tilemaps["Overlay"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.overlayTileNames[overlayType]]
        );
    }


    private void ConstructSolid(Vector2Int position)
    {
        tilemaps["Collision"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.overlayTileNames[OverlayType.Collision]]
        );
    }



    // Helper Methods

    public void SelectCell(Vector2Int position)
    {
        selectedCell = position;

        ConstructOverlay(selectedCell, OverlayType.Selection);
    }


    public void ClearSelection()
    {
        tilemaps["Overlay"].SetTile(
            new Vector3Int(selectedCell.x, selectedCell.y, 0),
            null
        );
    }


    public CellData GetCellData(int x, int y)
    {
        return mapData.cells[MapUtil.CoordsToIndex(x, y)];
    }
}
