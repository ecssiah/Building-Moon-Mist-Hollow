using UnityEngine;

namespace MMH
{
    public class EntityFactory : MonoBehaviour
    {
        private static GameObject citizenPrefab;

        private float nextEntityHeight;

        void Awake()
        {
            citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

            nextEntityHeight = 0;
        }


        public GameObject CreateCitizenObject(string name, Vector2Int position, GameObject parent = null)
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(position);

            nextEntityHeight += Info.Entity.HeightSpacing;

            GameObject newCitizenObject = Instantiate(
                citizenPrefab, new Vector3(worldPosition.x, worldPosition.y, nextEntityHeight), Quaternion.identity
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


