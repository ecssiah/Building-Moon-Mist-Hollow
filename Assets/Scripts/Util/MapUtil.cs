using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        return EntranceExistsAt(new Vector2Int(x, y), roomData);
    }


    public static bool EntranceExistsAt(Vector2Int position, RoomData roomData)
    {
        foreach (EntranceData entranceData in roomData.entrances)
        {
            if (entranceData.bounds.Contains(position))
            {
                return true;
            }
        }

        return false;
    }


    public static Vector2Int GetRandomMapPosition()
    {
        return new Vector2Int(
            Random.Range(-MapInfo.MapSize, MapInfo.MapSize),
            Random.Range(-MapInfo.MapSize, MapInfo.MapSize)
        );
    }


    public static Vector2Int GetRandomBorderPosition(RectInt bounds, bool includeCorners = false)
    {
        int cornerModifier = includeCorners ? 0 : 1;

        switch (Random.Range(0, 4))
        {
            case 0:
                Vector2Int topWallPosition = new Vector2Int(
                    Random.Range(bounds.xMin + cornerModifier, bounds.xMax - cornerModifier),
                    bounds.yMax
                );

                return topWallPosition;
            case 1:
                Vector2Int bottomWallPosition = new Vector2Int(
                    Random.Range(bounds.xMin + cornerModifier, bounds.xMax - cornerModifier),
                    bounds.yMin
                );

                return bottomWallPosition;
            case 2:
                Vector2Int leftWallPosition = new Vector2Int(
                    bounds.xMin,
                    Random.Range(bounds.yMin + cornerModifier, bounds.yMax - cornerModifier)
                );

                return leftWallPosition;
            case 3:
                Vector2Int rightWallPosition = new Vector2Int(
                    bounds.xMax,
                    Random.Range(bounds.yMin + cornerModifier, bounds.yMax - cornerModifier)
                );

                return rightWallPosition;
            default:
                return new Vector2Int(bounds.xMin + 1, bounds.yMin);
        }
    }
}
