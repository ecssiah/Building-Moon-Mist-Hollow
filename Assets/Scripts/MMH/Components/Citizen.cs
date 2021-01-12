using UnityEngine;

namespace MMH
{
    public class Citizen : MonoBehaviour
    {
        public Data.Entity Entity;
        public Data.Id Id;
        public Data.Inventory Inventory;


        void Awake()
        {
            gameObject.AddComponent<CitizenAnimator>();
            gameObject.AddComponent<CitizenMovement>();
        }


        public override string ToString()
        {
            string output = $"{Id.FullName} - {Entity.Position}";

            return output;
        }
    }
}

