using UnityEngine;

public struct MapInfo
{
    public static readonly int Size = 48;
    public static readonly int Width = 2 * Size + 1;

    public static readonly int SeedRoomSize = 4;
    public static readonly int NumberOfSeedRooms = 16;

    public static RectInt WorldBoundary = new RectInt(
        -Size - 1, -Size - 1, Width + 1, Width + 1
    );


}
