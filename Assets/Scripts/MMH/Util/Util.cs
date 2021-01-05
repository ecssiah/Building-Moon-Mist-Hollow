using System;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Util
{
    public static T RandomEnumValue<T>()
    {
        Array values = Enum.GetValues(typeof(T));

        return (T)values.GetValue(Random.Range(0, values.Length));
    }


    public static Vector2 RandomIsoDirection()
    {
        Vector2 newIsoDirection = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));

        return MapUtil.IsoToWorld(newIsoDirection);
    }
}