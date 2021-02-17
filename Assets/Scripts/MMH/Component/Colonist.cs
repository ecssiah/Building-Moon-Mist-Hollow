using Unity.Mathematics;
using UnityEngine;

namespace MMH.Component
{
    public class Colonist : MonoBehaviour
    {
        public Type.Behavior Behavior;

        public Data.Id Id;
        public Data.Entity Entity;
        public Data.Path Path;
        public Data.Inventory Inventory;

        private Pathfinding pathfinding;

        private void Awake()
        {
            gameObject.AddComponent<ColonistAnimator>();
            gameObject.AddComponent<ColonistMovement>();

            pathfinding = gameObject.AddComponent<Pathfinding>();
        }


        public void FindPath(int2 destination)
        {
            Path = pathfinding.FindPath(Entity.Position, destination);
        }


        public override string ToString()
        {
            string output = $"{Id.FullName} - {Entity.Position}";

            return output;
        }
    }
}

