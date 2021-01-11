using UnityEngine;

namespace MMH
{
    namespace System
    {
        public class EntitySystem : MonoBehaviour
        {
            private MapSystem mapSystem;
            private PathfindingSystem pathfindingSystem;

            private Data.Population population;

            private GameObject entitiesObject;
            private GameObject citizenPrefabObject;

            private float nextEntityHeight;


            void Awake()
            {
                mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
                pathfindingSystem = GameObject.Find("PathfindingSystem").GetComponent<PathfindingSystem>();

                entitiesObject = GameObject.Find("Entities");

                citizenPrefabObject = Resources.Load<GameObject>("Prefabs/Citizen");
            }


            void Start()
            {
                for (int i = 0; i < Info.Entity.NumberOfSeedCitizens; i++)
                {
                    Vector2Int position = Util.Map.GetRandomMapPosition();

                    while (mapSystem.GetCell(position).Solid)
                    {
                        position = Util.Map.GetRandomMapPosition();
                    }

                    GenerateCitizen(position);
                }
            }


            private void GenerateCitizen(Vector2Int position)
            {
                Type.Group groupType = Util.Misc.RandomEnumValue<Type.Group>();

                string citizenName = NameGenerator.GetName(groupType);

                GameObject citizenGameObject = CreateCitizenObject(citizenName, position, entitiesObject);

                Citizen citizen = citizenGameObject.AddComponent<Citizen>();

                citizen.Entity = new Data.Entity
                {
                    GameObject = citizenGameObject,
                    Speed = 0f,
                    Position = new Vector2(position.x, position.y),
                    Direction = new Vector2(0, 0),
                };

                citizen.Id = new Data.Id
                {
                    FullName = citizenName,
                    Number = population.NextId++,
                    PopulationType = Type.Population.Citizen,
                    GroupType = groupType,
                };
            }


            private GameObject CreateCitizenObject(string name, Vector2Int position, GameObject parent = null)
            {
                Vector2 worldPosition = Util.Map.IsoToWorld(position);

                nextEntityHeight -= Info.Entity.HeightSpacing;

                GameObject newCitizenObject = Instantiate(
                    citizenPrefabObject,
                    new Vector3(worldPosition.x, worldPosition.y, nextEntityHeight),
                    Quaternion.identity
                );

                newCitizenObject.name = name;

                if (parent != null)
                {
                    newCitizenObject.transform.parent = parent.transform;
                }

                return newCitizenObject;
            }


            public Data.Path RequestPath(Vector2Int start, Vector2Int end)
            {
                return pathfindingSystem.FindPath(start, end);
            }
        }
    }
}

