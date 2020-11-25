using System;
using UnityEngine;

public static class ConversionUtilities
{
    public static Vector3 CartesianToIso(Vector3 cartesianVector)
    {
        return new Vector3(
            cartesianVector.x - cartesianVector.y,
            (cartesianVector.x + cartesianVector.y) / 2,
            -10
        );
    }

    public static Vector3 ToIsoGrid(Vector3 screenVector)
    {
        Vector3 isoVector = new Vector3(
            screenVector.x - 2 * screenVector.y,
            screenVector.x + 2 * screenVector.y,
            -10
        );

        Vector3 isoGridVector = new Vector3(Mathf.Floor(isoVector.x) + 1, Mathf.Floor(isoVector.y), -10);

        return isoGridVector;
    }
}