﻿using UnityEngine;

public static class MapUtil
{
    public static Vector2 WorldToIso(Vector2 worldPosition)
    {
        return new Vector2(
            worldPosition.x + 2 * worldPosition.y,
            -worldPosition.x + 2 * worldPosition.y
        );
    }


    public static Vector2Int WorldToIsoGrid(Vector2 worldPosition)
    {
        Vector2 isoVector = WorldToIso(worldPosition);

        return new Vector2Int(
            (int)Mathf.Floor(isoVector.x),
            (int)Mathf.Floor(isoVector.y)
        );
    }


    public static Vector2 IsoToWorld(Vector2 isoVector)
    {
        var matrixProduct = new Vector2(
            2 * isoVector.x - 2 * isoVector.y,
            1 * isoVector.x + 1 * isoVector.y
        );

        return (1 / 4f) * matrixProduct;
    }


    public static Vector2Int ScreenToIsoGrid(Vector2 screenPosition)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.y += 0.25f;

        return WorldToIsoGrid(worldPosition);
    }


    public static Vector2 WorldToScreen(Vector2 worldPosition)
    {
        return RectTransformUtility.WorldToScreenPoint(
            Camera.main, new Vector3(worldPosition.x, worldPosition.y, 0)
        );
    }
}
