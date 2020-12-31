using UnityEngine;

public struct MapInfo
{
    public static readonly int Size = 24;
    public static readonly int Width = 2 * Size + 1;

    public static readonly int SeedRoomSize = 2;
    public static readonly int NumberOfSeedRooms = 16;
    public static readonly int MaximumExpansionAttempts = 1000;

    public static RectInt WorldBoundary = new RectInt(
        -Size - 1, -Size - 1, Width + 1, Width + 1
    );
}
