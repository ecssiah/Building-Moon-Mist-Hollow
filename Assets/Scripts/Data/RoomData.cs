using System.Collections.Generic;
using UnityEngine;

public struct RoomData
{
    public RectInt bounds;

    public bool fill;

    public GroundType groundType;
    public WallType wallType;
    public OverlayType overlayType;

    public List<EntranceData> entrances;
}