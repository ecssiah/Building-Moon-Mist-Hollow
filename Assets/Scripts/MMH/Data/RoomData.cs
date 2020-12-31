using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RoomData
{
    public RectInt Bounds;

    public bool Fill;

    public GroundType GroundType;
    public WallType WallType;
    public OverlayType OverlayType;

    public IList<EntranceData> Entrances;
}