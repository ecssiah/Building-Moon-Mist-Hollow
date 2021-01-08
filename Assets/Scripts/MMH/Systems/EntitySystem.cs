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

            private NameGenerator nameGenerator;
            private EntityFactory entityFactory;

            private GameObject entitiesObject;


            void Awake()
            {
                mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
                pathfindingSystem = GameObject.Find("PathfindingSystem").GetComponent<PathfindingSystem>();

                entitiesObject = GameObject.Find("Entities");

                nameGenerator = gameObject.AddComponent<NameGenerator>();
                entityFactory = gameObject.AddComponent<EntityFactory>();

                Physics2D.IgnoreLayerCollision(
                    LayerMask.NameToLayer("Citizens"),
                    LayerMask.NameToLayer("Citizens"),
                    true
                );
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

                string citizenName = nameGenerator.GetName(groupType);

                GameObject citizenGameObject = entityFactory.CreateCitizenObject(
                    citizenName, position, entitiesObject
                );

                Citizen citizen = citizenGameObject.AddComponent<Citizen>();

                citizen.Entity = new Data.Entity
                {
                    GameObject = citizenGameObject,
                    Speed = 0f,
                    Position = new Vector2(position.x, position.y),
                    Direction = new Vector2(1, 0),
                };

                citizen.Id = new Data.Id
                {
                    FullName = citizenName,
                    Number = population.NextId++,
                    PopulationType = Type.Population.Citizen,
                    GroupType = groupType,
                };
            }


            public Data.Path RequestPath(Vector2Int start, Vector2Int end)
            {
                return pathfindingSystem.FindPath(start, end);
            }
        }
    }
}

