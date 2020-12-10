using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem: MonoBehaviour
{
    private const int MapSize = 100;

    private MapData mapData;

    private Tile selectionTile;

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


    void Start()
    {
    }


    void Update()
    {
        
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

        Tilemap[] tilemapsArray = GameObject.Find("Map").GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }
    }


    public CellData GetCellData(int x, int y)
    {
        return mapData.cells[0];
    }


    public void SelectCell(Vector3Int position)
    {
        ResetSelection();

        selectedCell = position;

        tilemaps["Overlay"].SetTile(position, tiles["Selection_1"]);
    }


    public void ResetSelection()
    {
        tilemaps["Overlay"].SetTile(selectedCell, null);
    }


    private void InitMap()
    {
        mapData = new MapData
        {
            cells = new CellData[(int)Mathf.Pow(2 * MapSize + 1, 2)]
        };

        for (int x = -MapSize; x <= MapSize; x++)
        {
            for (int y = -MapSize; y <= MapSize; y++)
            {
                CellData newCellData = new CellData();

                mapData.cells[CoordsTo1DIndex(x, y)] = newCellData;
            }
        }
    }


    private void ConstructMap()
    {

    }


    public int CoordsTo1DIndex(int x, int y)
    {
        return (x + MapSize) + (2 * MapSize + 1) * (y + MapSize);
    }

}
