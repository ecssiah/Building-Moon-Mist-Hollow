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

        InitCells();
        InitGround();
        InitBuildings();
    }


    private void InitCells()
    {
        for (int i = 0; i < mapData.cells.Length; i++)
        {
            mapData.cells[i] = new CellData();
        }
    }


    private void InitGround()
    {
        for (int x = -MapSize; x <= MapSize; x++)
        {
            for (int y = -MapSize; y <= MapSize; y++)
            {
                mapData.cells[CoordsToIndex(x, y)].cellType = CellType.Grass;
            }
        }

        mapData.cells[CoordsToIndex(0, 0)].cellType = CellType.Water;
    }


    private void InitBuildings()
    {
        //for (int x = -4; x <= 4; x++)
        //{
        //    for (int y = -4; y <= 4; y++)
        //    {
        //        if (x == -4 || x == 4 || y == -4 || y == 4)
        //        {
        //            mapData.cells[CoordsToIndex(x, y)].buildingType = BuildingType.StoneWall;
        //        }
        //    }
        //}
        mapData.cells[CoordsToIndex(2, 2)].buildingType = BuildingType.StoneWall;
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
            CellData cellData = mapData.cells[i];
            Vector2Int position = IndexToCoords(i);


            if (cellData.cellType != CellType.None)
            {
                SetCell(position, cellData.cellType);
            }

            if(cellData.buildingType != BuildingType.None)
            {
                Debug.Log(position);
                SetBuilding(position, cellData.buildingType);
            }
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


    private void SetBuilding(Vector2Int position, BuildingType buildingType)
    {
        tilemaps["Buildings"].SetTile(
            new Vector3Int(position.x, position.y, 3),
            tiles[buildingTileNames[buildingType]]
        );
    }


    private void SetCell(Vector2Int position, CellType cellType)
    {
        tilemaps["Ground"].SetTile(
            new Vector3Int(position.x, position.y, 0),
            tiles[cellTileNames[cellType]]
        );
    }
}
