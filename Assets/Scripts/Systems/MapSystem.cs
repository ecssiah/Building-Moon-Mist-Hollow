using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem : MonoBehaviour
{
    private MapData mapData;

    private Dictionary<string, Tile> tiles;
    private Dictionary<string, Tilemap> tilemaps;

    private Vector3Int selectedCell;

    private Dictionary<CellType, string> cellTileNames;
    private Dictionary<BuildingType, string> buildingTileNames;


    void Awake()
    {
        selectedCell = new Vector3Int();

        InitTiles();
        InitTilemaps();

        InitMap();


    }


    void Start()
    {
        
    }


    void Update()
    {
        
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

        cellTileNames = new Dictionary<CellType, string>
        {
            [CellType.Grass] = "Dirt_Grass_C",
            [CellType.Stone] = "Stone_A",
            [CellType.Water] = "Water",
        };

        buildingTileNames = new Dictionary<BuildingType, string>
        {
            [BuildingType.StoneWall] = "Brick_C",
            [BuildingType.WoodWall] = "Wood_A",
        };
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

    }
}
