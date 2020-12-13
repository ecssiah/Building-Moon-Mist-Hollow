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


    public static Vector3Int ToIsoGrid(Vector3 screen)
    {
        Vector3 isoVector = new Vector3(
            screen.x + 2 * screen.y,
            -screen.x + 2 * screen.y,
            0
        );

        Vector3Int isoGridVector = new Vector3Int(
            (int)Mathf.Floor(isoVector.x),
            (int)Mathf.Floor(isoVector.y),
            0
        );

        return isoGridVector;
    }


    public static Vector3Int GetWorldPoint(Vector3 atPosition)
    {
        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(atPosition);
        selectedPosition.y += 0.25f;

        return Utilities.ToIsoGrid(selectedPosition);
    }
}
