using Unity.Mathematics;
using UnityEngine;

namespace MMH.Data
{
    public class Entity
    {
        public GameObject GameObject;

        public int2 Position;

        public float Speed;
        public Type.Direction Direction;


        public Entity()
        {
            GameObject = null;

            Position = new int2();
            Speed = Info.Entity.DefaultWalkSpeed;
            Direction = Type.Direction.SW;
        }
    }
}
