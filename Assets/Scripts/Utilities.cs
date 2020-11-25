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


    public static Vector3 ToIsoGrid(Vector3 screen)
    {
        Vector3 isoVector = new Vector3(
            screen.x - 2 * screen.y,
            screen.x + 2 * screen.y,
            -10
        );

        Vector3 isoGridVector = new Vector3(
            Mathf.Floor(isoVector.x) + 1, Mathf.Floor(isoVector.y), 10
        );

        return isoGridVector;
    }
}
