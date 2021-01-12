using UnityEngine;


namespace MMH
{
    namespace Data
    {
        public struct Entity
        {
            public GameObject GameObject;

            public Vector2Int Position;

            public float Speed;
            public Type.Direction Direction;
        }
    }
}
