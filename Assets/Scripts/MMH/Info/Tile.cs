using System.Collections.Generic;

namespace MMH.Info
{
    public struct Tile
    {
        public static readonly Dictionary<Type.Ground, string> groundTileNames;
        public static readonly Dictionary<Type.Wall, string> wallTileNames;
        public static readonly Dictionary<Type.Overlay, string> overlayTileNames;


        static Tile()
        {
            groundTileNames = new Dictionary<Type.Ground, string>
            {
                [Type.Ground.Grass] = "Dirt_Grass_C",
                [Type.Ground.Stone] = "Stone_A",
                [Type.Ground.Water] = "Water",
                [Type.Ground.Wood] = "Wood_A"
            };

            wallTileNames = new Dictionary<Type.Wall, string>
            {
                [Type.Wall.StoneWall] = "Brick_C",
                [Type.Wall.WoodWall] = "Wood_A",
            };

            overlayTileNames = new Dictionary<Type.Overlay, string>
            {
                [Type.Overlay.Collision] = "Collision_1",
                [Type.Overlay.Selection] = "Selection_1",
            };
        }
    }
}
