using UnityEngine;

namespace MMH
{
    public class EntityFactory : MonoBehaviour
    {
        private static GameObject citizenPrefab;


        void Awake()
        {
            citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");
        }


        public GameObject CreateCitizenObject(string name, Vector2Int position, GameObject parent = null)
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(position);

            GameObject newCitizenObject = Instantiate(
                citizenPrefab,
                new Vector3(worldPosition.x, worldPosition.y, 0),
                Quaternion.identity
            );

            newCitizenObject.name = name;
            newCitizenObject.layer = LayerMask.NameToLayer("Citizens");

            if (parent != null)
            {
                newCitizenObject.transform.parent = parent.transform;
            }

            return newCitizenObject;
        }
    }
}


