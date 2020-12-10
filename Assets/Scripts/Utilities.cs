using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 CartesianToIso(Vector3 cartesianVector)
    {
        return new Vector3(
            cartesianVector.x - cartesianVector.y,
            (cartesianVector.x + cartesianVector.y) / 2,
            cartesianVector.z
        );
    }


    public static Vector3 ScreenToIso(Vector3 screen)
    {
        return new Vector3(
            screen.x + 2 * screen.y,
            -screen.x + 2 * screen.y,
            0
        );
    }


    public static Vector3Int ScreenToIsoGrid(Vector3 screen)
    {
        Vector3 IsoVector = ScreenToIso(screen);

        return new Vector3Int(
            (int)Mathf.Floor(IsoVector.x),
            (int)Mathf.Floor(IsoVector.y),
            0
        );
    }


    public static Vector3 WorldToScreen(Vector3 worldPosition)
    {
        return RectTransformUtility.WorldToScreenPoint(
            Camera.main, worldPosition
        );
    }


    public static Vector3Int GetWorldPoint(Vector3 atPosition)
    {
        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(atPosition);
        selectedPosition.y += 0.25f;

        return Utilities.ScreenToIsoGrid(selectedPosition);
    }
}
