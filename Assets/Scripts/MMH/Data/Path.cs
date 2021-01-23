using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Data
{
    public struct Path
    {
        public float Progress;
        public List<int2> NodePositions;

        public bool Valid => NodePositions.Count > 0;
    }
}