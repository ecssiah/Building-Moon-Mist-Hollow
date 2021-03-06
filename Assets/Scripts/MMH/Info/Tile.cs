﻿using System.Collections.Generic;

namespace MMH.Info
{
    public struct Tile
    {
        public static readonly Dictionary<Type.Ground, string> groundTileNames;
        public static readonly Dictionary<Type.Structure, string> structureTileNames;
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

            structureTileNames = new Dictionary<Type.Structure, string>
            {
                [Type.Structure.Stone_Wall] = "Brick_C",
                [Type.Structure.Wood_Wall] = "Wood_A",
                [Type.Structure.Guy_Flag] = "Guy_Flag",
                [Type.Structure.Kailt_Flag] = "Kailt_Flag",
                [Type.Structure.Taylor_Flag] = "Taylor_Flag",
            };

            overlayTileNames = new Dictionary<Type.Overlay, string>
            {
                [Type.Overlay.Collision] = "Collision_1",
                [Type.Overlay.Selection] = "Selection_1",
            };
        }
    }
}
