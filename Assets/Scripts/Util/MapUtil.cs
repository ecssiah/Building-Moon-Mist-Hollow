﻿using UnityEngine;

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


    public static Vector3 WorldToIso(Vector3 worldPosition)
    {
        return new Vector3(
            worldPosition.x + 2 * worldPosition.y,
            -worldPosition.x + 2 * worldPosition.y,
            0
        );
    }


    public static Vector3Int WorldToIsoGrid(Vector3 screenPosition)
    {
        Vector3 isoVector = WorldToIso(screenPosition);

        return new Vector3Int(
            (int)Mathf.Floor(isoVector.x),
            (int)Mathf.Floor(isoVector.y),
            0
        );
    }


    public static Vector3 WorldToScreen(Vector3 worldPosition)
    {
        return RectTransformUtility.WorldToScreenPoint(
            Camera.main, worldPosition
        );
    }


    public static Vector3Int ScreenToIsoGrid(Vector3 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.y += 0.25f;

        return WorldToIsoGrid(worldPosition);
    }


    public static Vector3 IsoToWorld(Vector3 isoVector)
    {
        var matrixProduct = new Vector3(
            2 * isoVector.x - 2 * isoVector.y,
            1 * isoVector.x + 1 * isoVector.y,
            0
        );

        return (1 / 4f) * matrixProduct;
    }
}