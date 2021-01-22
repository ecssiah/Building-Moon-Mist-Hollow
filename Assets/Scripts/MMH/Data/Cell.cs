using System;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.Data
{
    [Serializable]
    public struct Cell
    {
        public int2 Position;

        public bool Solid;

        public Type.Ground GroundType;
        public Type.Structure StructureType;
        public Type.Overlay OverlayType;
    }
}
