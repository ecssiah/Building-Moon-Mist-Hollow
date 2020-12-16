using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem: MonoBehaviour
{
    private MapData mapData;

    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    private Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();

    private Vector3Int selectedCell = new Vector3Int();


    void Awake()
    {
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
        foreach (Tile tile in Resources.LoadAll<Tile>("Tiles"))
        {
            tiles[tile.name] = tile;
        }
    }


    private void InitTilemaps()
    {
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
        InitCells();
        InitGround();
        InitBuildings();
    }


    private void InitCells()
    {
        mapData = new MapData
        {
            cells = new CellData[(int)Mathf.Pow(MapInfo.MapWidth, 2)]
        };
    }


    private void InitGround()
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


    private void InitBuildings()
    {
        SetupBuilding(4, 4, BuildingType.StoneWall);
        SetupBuilding(-4, 4, BuildingType.StoneWall);
        SetupBuilding(4, -4, BuildingType.StoneWall);
        SetupBuilding(-4, -4, BuildingType.StoneWall);
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
                ConstructCell(position, cellData.cellType);
            }

            if(cellData.buildingType != BuildingType.None)
            {
                ConstructBuilding(position, cellData.buildingType);
            }
        }
    }


    private int CoordsToIndex(Vector2Int position)
    {
        return CoordsToIndex(position.x, position.y);
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


    private void SetupCell(int x, int y, CellType cellType)
    {
        SetupCell(new Vector2Int(x, y), cellType);
    }


    private void SetupCell(Vector2Int position, CellType cellType)
    {
        mapData.cells[CoordsToIndex(position)].cellType = cellType;
    }


    private void SetupBuilding(int x, int y, BuildingType buildingType)
    {
        SetupBuilding(new Vector2Int(x, y), buildingType);
    }


    private void SetupBuilding(Vector2Int position, BuildingType buildingType)
    {
        mapData.cells[CoordsToIndex(position)].buildingType = buildingType;
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
}
