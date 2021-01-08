using UnityEngine;


namespace MMH
{
    namespace Data
    {
        public struct Entity
        {
            public GameObject GameObject;

            public Vector2 Position;
            public Vector2Int GridPosition => new Vector2Int((int)Position.x, (int)Position.y);

            public float Speed;
            public Vector2 Direction;
        }
    }
}
