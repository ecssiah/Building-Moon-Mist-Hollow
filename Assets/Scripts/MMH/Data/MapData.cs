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
}       
