using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 WorldToIso(Vector3 worldPosition)
    {
        return new Vector3(
            worldPosition.x + 2 * worldPosition.y,
            -worldPosition.x + 2 * worldPosition.y,
            0
        );
    }


    public static Vector3Int WorldToIsoGrid(Vector3 screen)
    {
        Vector3 IsoVector = WorldToIso(screen);

        return new Vector3Int(
            (int)Mathf.Floor(IsoVector.x),
            (int)Mathf.Floor(IsoVector.y),
            0
        );
    }


    public static Vector3 IsoToWorld(Vector3 isoVector)
    {
        return (1 / 4f) * new Vector3(
            2 * isoVector.x - 2 * isoVector.y,
            isoVector.x + isoVector.y, 0
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
        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        selectedPosition.y += 0.25f;

        return Utilities.WorldToIsoGrid(selectedPosition);
    }
}
