using System;
using UnityEngine;


public struct CellData
{
    public Vector2Int position;

    public bool solid;

    public CellType cellType;                     
    public BuildingType buildingType;
    public OverlayType overlayType;
}
