using System;
using UnityEngine;

[Serializable]
public struct CellData
{
    public Vector2Int Position;

    public bool Solid;

    public GroundType GroundType;
    public WallType WallType;
    public OverlayType OverlayType;
}
