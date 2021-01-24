using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Info
{
    public struct Map
    {
        public static readonly int Size = 32;
        public static readonly int Width = 2 * Size + 1;

        public static readonly bool ShowCollision = false;

        public static readonly int SeedRoomSize = 3;
        public static readonly int NumberOfSeedRooms = 32;
        public static readonly int MaximumExpansionAttempts = 20;

        public static readonly int PathWidth = Width / 12;

        public static readonly RectInt WorldBoundary = new RectInt(
            -Size - 1, -Size - 1, Width + 1, Width + 1
        );

        public static readonly Dictionary<Type.Direction, int2> Directions = new Dictionary<Type.Direction, int2>
        {
            [Type.Direction.EE] = new int2(+1, +0),
            [Type.Direction.NE] = new int2(+1, +1),
            [Type.Direction.NN] = new int2(+0, +1),
            [Type.Direction.NW] = new int2(-1, +1),
            [Type.Direction.WW] = new int2(-1, +0),
            [Type.Direction.SW] = new int2(-1, -1),
            [Type.Direction.SS] = new int2(+0, -1),
            [Type.Direction.SE] = new int2(+1, -1),
        };
    }
}
