using Unity.Mathematics;
using UnityEngine;

namespace MMH.Component
{
    public class Colonist : MonoBehaviour
    {
        public Data.Id Id;
        public Data.Entity Entity;
        public Data.Praxis Praxis;
        public Data.Path Path;
        public Data.Inventory Inventory;

        private Pathfinding pathfinding;

        private void Awake()
        {
            gameObject.AddComponent<ColonistAnimator>();
            gameObject.AddComponent<ColonistMovement>();

            Id = new Data.Id();
            Entity = new Data.Entity();
            Praxis = new Data.Praxis();
            Path = new Data.Path();
            Inventory = new Data.Inventory();

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

