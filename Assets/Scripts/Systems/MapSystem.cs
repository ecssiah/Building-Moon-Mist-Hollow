using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem: MonoBehaviour
{
    private const int MapSize = 100;
    private const int MapWidth = 2 * MapSize + 1;

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

        cellTileNames = new Dictionary<CellType, string>
        {
            [CellType.Grass] = "Dirt_Grass_C",
            [CellType.Stone] = "Stone_A",
            [CellType.Water] = "Water"
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

        Tilemap[] tilemapsArray = GameObject.Find("Map").GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }
    }


    public CellData GetCellData(int x, int y)
    {
        return mapData.cells[CoordsToIndex(x, y)];
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


    private void InitMap()
    {
        mapData = new MapData
        {
            cells = new CellData[(int)Mathf.Pow(MapWidth, 2)]
        };

        for (int x = -MapSize; x <= MapSize; x++)
        {
            for (int y = -MapSize; y <= MapSize; y++)
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
                        newCellData.solid = true;
                        break;
                    case 2:
                        newCellData.cellType = CellType.Water;
                        break;
                }

                mapData.cells[CoordsToIndex(x, y)] = newCellData;
            }
        }
    }


    private void InitGround()
    {

    }


    private void InitBuildings()
    {

    }


    private void InitOverlay()
    {

    }


    private void InitCollision()
    {

    }


    private void ConstructMap()
    {
        for (int i = 0; i < mapData.cells.Length; i++)
        {
            SetCell(IndexToCoords(i), mapData.cells[i].cellType);
        }
    }


    private int CoordsToIndex(int x, int y)
    {
        return (x + MapSize) + MapWidth * (y + MapSize);
    }


    private Vector2Int IndexToCoords(int i)
    {
        return new Vector2Int(
            (i % MapWidth) - MapSize, (i / MapWidth) - MapSize
        );
    }


    private void SetCell(int x, int y, CellType cellType)
    {
        tilemaps["Ground"].SetTile(new Vector3Int(x, y, 0), tiles[cellTileNames[cellType]]);
    }


    private void SetCell(Vector2Int position, CellType cellType)
    {
        SetCell(position.x, position.y, cellType);
    }
}
