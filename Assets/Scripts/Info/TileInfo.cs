using System.Collections.Generic;

public struct TileInfo
{
    public static readonly Dictionary<GroundType, string> groundTileNames;
    public static readonly Dictionary<WallType, string> wallTileNames;
    public static readonly Dictionary<OverlayType, string> overlayTileNames;


    static TileInfo()
    {
        groundTileNames = new Dictionary<GroundType, string>
        {
            [GroundType.Grass] = "Dirt_Grass_C",
            [GroundType.Stone] = "Stone_A",
            [GroundType.Water] = "Water",
            [GroundType.Wood] = "Wood_A"
        };

        wallTileNames = new Dictionary<WallType, string>
        {
            [WallType.StoneWall] = "Brick_C",
            [WallType.WoodWall] = "Wood_A",
        };

        overlayTileNames = new Dictionary<OverlayType, string>
        {
            [OverlayType.Collision] = "Collision_1",
            [OverlayType.Selection] = "Selection_1",
        };
    }
}
