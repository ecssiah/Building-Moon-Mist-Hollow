using System;
using UnityEngine;

public struct MapUtil
{
    public static int CoordsToIndex(Vector2Int position)
    {
        return CoordsToIndex(position.x, position.y);
    }


    public static int CoordsToIndex(int x, int y)
    {
        return (x + MapInfo.MapSize) + MapInfo.MapWidth * (y + MapInfo.MapSize);
    }


    public static Vector2Int IndexToCoords(int i)
    {
        return new Vector2Int(
            (i % MapInfo.MapWidth) - MapInfo.MapSize,
            (i / MapInfo.MapWidth) - MapInfo.MapSize
        );
    }


    public static Vector2 WorldToIso(Vector2 worldPosition)
    {
        return new Vector2(
            worldPosition.x + 2 * worldPosition.y,
            -worldPosition.x + 2 * worldPosition.y
        );
    }


    public static Vector2Int WorldToIsoGrid(Vector2 screenPosition)
    {
        Vector2 isoVector = WorldToIso(screenPosition);

        return new Vector2Int(
            (int)Mathf.Floor(isoVector.x),
            (int)Mathf.Floor(isoVector.y)
        );
    }


    public static Vector2 WorldToScreen(Vector2 worldPosition)
    {
        return RectTransformUtility.WorldToScreenPoint(
            Camera.main, new Vector3(worldPosition.x, worldPosition.y, 0)
        );
    }


    public static Vector2Int ScreenToIsoGrid(Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.y += 0.25f;

        return WorldToIsoGrid(worldPosition);
    }


    public static Vector2 IsoToWorld(Vector2 isoVector)
    {
        var matrixProduct = new Vector2(
            2 * isoVector.x - 2 * isoVector.y,
            1 * isoVector.x + 1 * isoVector.y
        );

        return (1 / 4f) * matrixProduct;
    }


    public static bool OnRectBoundary(int x, int y, RectInt rect)
    {
        bool topEdge = y == rect.yMax;
        bool bottomEdge = y == rect.yMin;
        bool leftEdge = x == rect.xMin;
        bool rightEdge = x == rect.xMax;

        return topEdge || bottomEdge || leftEdge || rightEdge;
    }


    public static bool OnMap(int x, int y)
    {
        return OnMap(new Vector2Int(x, y));
    }


    public static bool OnMap(Vector2Int position)
    {
        RectInt mapBoundary = new RectInt
        {
            x = -MapInfo.MapSize,
            y = -MapInfo.MapSize,
            width = MapInfo.MapWidth,
            height = MapInfo.MapWidth,
        };

        return mapBoundary.Contains(position);
    }


    public static bool EntranceExistsAt(int x, int y, RoomData roomData)
    {
        foreach (EntranceData entranceData in roomData.entrances)
        {
            if (entranceData.bounds.Contains(new Vector2Int(x, y))) return true;
        }

        return false;
    }
}
