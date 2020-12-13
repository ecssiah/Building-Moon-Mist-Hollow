using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellData
{
    public Vector3Int position;

    public bool solid;

    public CellType cellType;
    public BuildingType buildingType;
}
