using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Util
{
    public struct Math
    {
        public static float2 Lerp2(float2 start, float2 end, float step)
        {
            return new float2(
                math.lerp(start.x, end.x, step),
                math.lerp(start.y, end.y, step)
            );
        }
    }
}

