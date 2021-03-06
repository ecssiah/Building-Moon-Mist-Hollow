﻿using Unity.Mathematics;
using UnityEngine;

namespace MMH.Data
{
    public struct Entity
    {
        public GameObject GameObject;

        public int2 Position;

        public float Speed;
        public Type.Direction Direction;
    }
}
