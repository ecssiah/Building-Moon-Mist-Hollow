using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MapData
{
    public bool ShowCollision;

    public CellData[] Cells;

    public List<RoomData> Rooms;
    public List<RectInt> Placeholders;


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
