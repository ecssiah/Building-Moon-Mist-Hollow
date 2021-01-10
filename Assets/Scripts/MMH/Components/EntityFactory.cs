using UnityEngine;

namespace MMH
{
    public class EntityFactory : MonoBehaviour
    {
        private static GameObject citizenPrefab;

        private float currentZValue;

        void Awake()
        {
            citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

            currentZValue = 0;
        }


        public GameObject CreateCitizenObject(string name, Vector2Int position, GameObject parent = null)
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(position);

            currentZValue += 0.001f;

            GameObject newCitizenObject = Instantiate(
                citizenPrefab, new Vector3(worldPosition.x, worldPosition.y, currentZValue), Quaternion.identity
            );

            newCitizenObject.name = name;

            if (parent != null)
            {
                newCitizenObject.transform.parent = parent.transform;
            }

            return newCitizenObject;
        }
    }
}


