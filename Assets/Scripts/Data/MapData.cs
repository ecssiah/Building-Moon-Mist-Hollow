using System.Collections.Generic;
using UnityEngine;

public struct MapData
{
    public bool showCollision;

    public CellData[] cells;

    public List<RoomData> rooms;
    public List<RectInt> placeholders;
}       
