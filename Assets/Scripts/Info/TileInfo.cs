using System.Collections.Generic;

public struct TileInfo
{
    public static readonly Dictionary<CellType, string> cellTileNames;
    public static readonly Dictionary<BuildingType, string> buildingTileNames;
    public static readonly Dictionary<OverlayType, string> overlayTileNames;

    static TileInfo()
    {
        cellTileNames = new Dictionary<CellType, string>
        {
            [CellType.Grass] = "Dirt_Grass_C",
            [CellType.Stone] = "Stone_A",
            [CellType.Water] = "Water",
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