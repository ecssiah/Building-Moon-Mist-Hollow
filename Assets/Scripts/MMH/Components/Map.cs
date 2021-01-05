using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public RoomBuilder RoomBuilder;

    public bool ShowCollision;

    public int Size;
    public int Width { get => 2 * Size + 1; }

    public CellData[] Cells;

    public List<RoomData> Rooms;
    public List<RectInt> Placeholders;


    void Awake()
    {
        RoomBuilder = gameObject.AddComponent<RoomBuilder>();

        Size = MapInfo.Size;
        ShowCollision = MapInfo.ShowCollision;
        Cells = new CellData[MapInfo.Width * MapInfo.Width];
        Rooms = new List<RoomData>(MapInfo.NumberOfSeedRooms);
        Placeholders = new List<RectInt>();
    }


    public CellData GetCell(int x, int y)
    {
        return Cells[MapUtil.CoordsToIndex(x, y)];
    }


    public CellData GetCell(Vector2Int position)
    {
        return GetCell(position.x, position.y);
    }


    public void SetCell(int x, int y, CellData cellData)
    {
        Cells[MapUtil.CoordsToIndex(x, y)] = cellData;
    }


    public void SetCell(Vector2Int position, CellData cellData)
    {
        SetCell(position.x, position.y, cellData);
    }

}
