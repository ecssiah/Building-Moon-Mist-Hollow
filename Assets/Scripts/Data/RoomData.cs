using UnityEngine;

public struct RoomData
{
    public RectInt bounds;

    public bool fill;

    public GroundType groundType;
    public BuildingType buildingType;
    public OverlayType overlayType;

    public EntranceData[] entrances;
}