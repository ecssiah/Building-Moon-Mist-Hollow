using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH.Data
{
    [Serializable]
    public struct Room
    {
        public RectInt Bounds;

        public bool Fill;

        public Type.Ground GroundType;
        public Type.Structure WallType;
        public Type.Overlay OverlayType;

        public List<Entrance> Entrances;
    }
}