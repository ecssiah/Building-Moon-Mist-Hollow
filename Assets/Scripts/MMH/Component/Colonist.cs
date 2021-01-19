using UnityEngine;

namespace MMH
{
    public class Colonist : EntityBase
    {
        public Data.Id Id;
        public Data.Inventory Inventory;

        public override void Awake()
        {
            gameObject.AddComponent<ColonistAnimator>();
            gameObject.AddComponent<ColonistMovement>();
        }


        public override string ToString()
        {
            string output = $"{Id.FullName} - {Entity.Position}";

            return output;
        }
    }
}

