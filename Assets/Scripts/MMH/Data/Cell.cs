using System;
using UnityEngine;

namespace MMH.Data
{
    [Serializable]
    public struct Cell
    {
        public Vector2Int Position;

        public bool Solid;

        public Type.Ground GroundType;
        public Type.Structure StructureType;
        public Type.Overlay OverlayType;
    }
}
