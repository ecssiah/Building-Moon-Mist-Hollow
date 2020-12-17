using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : MonoBehaviour
{
    private MapData mapData;

    private Dictionary<string, Tile> tiles;
    private Dictionary<string, Tilemap> tilemaps;

    private Vector3Int selectedCell;


    void Awake()
    {
        selectedCell = new Vector3Int();

        InitTiles();
        InitTilemaps();

        InitMap();

        ConstructMap();
    }


    public void SelectCell(Vector3Int position)
    {
        selectedCell = position;

        tilemaps["Overlay"].SetTile(position, tiles["Selection_1"]);
    }


    public void ClearSelection()
    {
        tilemaps["Overlay"].SetTile(selectedCell, null);
    }


    private void InitTiles()
    {
        tiles = new Dictionary<string, Tile>();

        Tile[] tilesArray = Resources.LoadAll<Tile>("Tiles");

        foreach (Tile tile in tilesArray)
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
    }


    private void InitMap()
    {
        mapData = new MapData
        {
            cells = new CellData[(int)Mathf.Pow(MapInfo.MapWidth, 2)]
        };

        for (int x = -MapInfo.MapSize; x <= MapInfo.MapSize; x++)
        {
            for (int y = -MapInfo.MapSize; y <= MapInfo.MapSize; y++)
            {
                CellData newCellData = new CellData();

                int roll = Random.Range(0, 3);

                switch (roll)
                {
                    case 0:
                        newCellData.cellType = CellType.Grass;
                        break;
                    case 1:
                        newCellData.cellType = CellType.Stone;
                        break;
                    case 2:
                        newCellData.cellType = CellType.Water;
                        break;
                }

                mapData.cells[CoordsToIndex(x, y)] = newCellData;
            }
        }
    }


    private void ConstructMap()
    {
        for (int i = 0; i < mapData.cells.Length; i++)
        {
            SetCell(IndexToCoords(i), mapData.cells[i].cellType);
        }
    }


    private void SetCell(Vector2Int position, CellType cellType)
    {
        tilemaps["Tiles"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[TileInfo.cellTileNames[cellType]]
        );
    }


    private int CoordsToIndex(int x, int y)
    {
        return (x + MapInfo.MapSize) + MapInfo.MapWidth * (y + MapInfo.MapSize);
    }


    private Vector2Int IndexToCoords(int i)
    {
        return new Vector2Int(
            (i % MapInfo.MapWidth) - MapInfo.MapSize, (i / MapInfo.MapWidth) - MapInfo.MapSize
        );
    }
}
