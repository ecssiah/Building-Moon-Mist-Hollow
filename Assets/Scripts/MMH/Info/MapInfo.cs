using UnityEngine;

public struct MapInfo
{
    public static readonly int Size = 2;
    public static readonly int Width = 2 * Size + 1;

    public static readonly int SeedRoomSize = 2;
    public static readonly int NumberOfSeedRooms = 0;
    public static readonly int MaximumExpansionAttempts = 20;

    public static readonly int PathWidth = Width / 12;

    public static RectInt WorldBoundary = new RectInt(
        -Size - 1, -Size - 1, Width + 1, Width + 1
    );
}
