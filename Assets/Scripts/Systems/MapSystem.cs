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
        SetupGround();
        SetupBuildings();
        SetupOverlay();
    }


    private void SetupGround()
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


    private void SetupBuildings()
    {

    }


    private void SetupOverlay()
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



    // Construction Methods

    private void ConstructMap()
    {
        for (int i = 0; i < mapData.cells.Length; i++)
        {
            ConstructCell(MapUtil.IndexToCoords(i), mapData.cells[i].cellType);
        }
    }


    private void ConstructCell(Vector2Int position, CellType cellType)
    {
        tilemaps["Tiles"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.cellTileNames[cellType]]
        );
    }



    // Helper Methods

    public void SelectCell(Vector2Int position)
    {
        selectedCell = position;

        tilemaps["Overlay"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles["Selection_1"]
        );
    }


    public void ClearSelection()
    {
        tilemaps["Overlay"].SetTile(
            new Vector3Int(selectedCell.x, selectedCell.y, 0),
            null
        );
    }
}
