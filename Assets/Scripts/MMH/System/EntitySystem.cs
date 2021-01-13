using UnityEngine;

namespace MMH.System
{
    public class EntitySystem : MonoBehaviour
    {
        private MapSystem mapSystem;
        private PathfindingSystem pathfindingSystem;

        private Data.Population population;

        private GameObject entitiesObject;
        private GameObject citizenPrefabObject;


        private void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
            pathfindingSystem = GameObject.Find("PathfindingSystem").GetComponent<PathfindingSystem>();

            entitiesObject = GameObject.Find("Entities");
            citizenPrefabObject = Resources.Load<GameObject>("Prefabs/Citizen");
        }


        private void Start()
        {
            for (int i = 0; i < Info.Entity.NumberOfSeedCitizens; i++)
            {
                Data.Cell cellData = mapSystem.GetFreeCell();

                GenerateCitizen(cellData.Position);
            }
        }


        private void GenerateCitizen(Vector2Int position)
        {
            Type.Group groupType = Util.Misc.RandomEnumValue<Type.Group>();

            GameObject citizenGameObject = GenerateCitizenObject(position);
            Citizen citizen = citizenGameObject.AddComponent<Citizen>();

            citizen.Entity = new Data.Entity
            {
                GameObject = citizenGameObject,
                Speed = 0f,
                Position = new Vector2Int(position.x, position.y),
                Direction = Type.Direction.S,
            };

            citizen.Id = new Data.Id
            {
                FullName = NameGenerator.GetName(groupType),
                Number = population.NextId++,
                PopulationType = Type.Population.Citizen,
                GroupType = groupType,
            };

            citizenGameObject.name = citizen.Id.FullName;
            citizenGameObject.transform.parent = entitiesObject.transform;
        }


        private GameObject GenerateCitizenObject(Vector2Int position)
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(position);

            GameObject newCitizenObject = Instantiate(
                citizenPrefabObject,
                new Vector3(worldPosition.x, worldPosition.y, 0),
                Quaternion.identity
            );

            return newCitizenObject;
        }


        public Data.Path RequestPath(Vector2Int start, Vector2Int end)
        {
            return pathfindingSystem.FindPath(start, end);
        }
    }
}

