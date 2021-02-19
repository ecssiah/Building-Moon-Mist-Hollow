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

            Path = new Data.Path();
            pathfinding = gameObject.AddComponent<Pathfinding>();
        }


        public bool HasPath()
        {
            return Path.Positions.Count > 0;
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

