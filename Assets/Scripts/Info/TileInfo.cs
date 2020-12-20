using System.Collections.Generic;

public struct TileInfo
{
    public static readonly Dictionary<GroundType, string> cellTileNames;
    public static readonly Dictionary<BuildingType, string> buildingTileNames;
    public static readonly Dictionary<OverlayType, string> overlayTileNames;


    static TileInfo()
    {
        cellTileNames = new Dictionary<GroundType, string>
        {
            [GroundType.Grass] = "Dirt_Grass_C",
            [GroundType.Stone] = "Stone_A",
            [GroundType.Water] = "Water"
        };

        buildingTileNames = new Dictionary<BuildingType, string>
        {
            [BuildingType.StoneWall] = "Brick_C",
            [BuildingType.WoodWall] = "Wood_A",
        };

        overlayTileNames = new Dictionary<OverlayType, string>
        {
            [OverlayType.Collision] = "Collision_1",
            [OverlayType.Selection] = "Selection_1",
        };
    }
}
