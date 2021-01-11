using UnityEngine;

namespace MMH
{
    namespace Info
    {
        public struct Map
        {
            public static readonly int Size = 24;
            public static readonly int Width = 2 * Size + 1;

            public static readonly bool ShowCollision = false;

            public static readonly int SeedRoomSize = 3;
            public static readonly int NumberOfSeedRooms = 12;
            public static readonly int MaximumExpansionAttempts = 20;

            public static readonly int PathWidth = Width / 12;

            public static RectInt WorldBoundary = new RectInt(
                -Size - 1, -Size - 1, Width + 1, Width + 1
            );
        }
    }
}
