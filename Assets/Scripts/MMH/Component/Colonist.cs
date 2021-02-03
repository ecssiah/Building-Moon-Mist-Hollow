using UnityEngine;

namespace MMH
{
    public class Colonist : MonoBehaviour
    {
        public Data.Entity Entity;

        public Data.Id Id;
        public Data.Path Path;
        public Data.Inventory Inventory;

        private void Awake()
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

