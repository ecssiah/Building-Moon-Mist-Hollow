using UnityEngine;

namespace MMH.Info
{
    public struct Map
    {
        public static readonly int Size = 32;
        public static readonly int Width = 2 * Size + 1;
        public static readonly int Area = Width * Width;

        public static readonly int PathWidth = Width / 12;

        public static RectInt Boundary = new RectInt(
            -Size - 1, -Size - 1,
            Width + 1, Width + 1
        );

        public static readonly bool ShowCollision = false;

        public static readonly int SeedRoomSize = 3;
        public static readonly int NumberOfSeedRooms = 32;
        public static readonly int MaximumExpansionAttempts = 20;
    }
}
