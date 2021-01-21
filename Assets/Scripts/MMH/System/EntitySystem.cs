using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH.System
{
    public class EntitySystem : MonoBehaviour, Handler.IEntityEventHandler
    {
        private MapSystem mapSystem;
        private PathfindingSystem pathfindingSystem;

        private Data.Population population;

        private GameObject entitiesObject;
        private GameObject colonistPrefab;


        private void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
            pathfindingSystem = GameObject.Find("PathfindingSystem").GetComponent<PathfindingSystem>();

            population = new Data.Population
            {
                NextId = 0,
                Colonists = new List<Colonist>(),
            };

            entitiesObject = GameObject.Find("Entities");
            colonistPrefab = Resources.Load<GameObject>("Prefabs/Colonist");
        }


        private void Start()
        {
            for (int i = 0; i < Info.Entity.NumberOfSeedColonists; i++)
            {
                Data.Cell cellData = mapSystem.GetFreeCell();

                GenerateColonist(cellData.Position);
            }
        }


        private void GenerateColonist(Vector2Int position)
        {
            Type.Group groupType = Util.Misc.RandomEnumValue<Type.Group>();

            GameObject colonistGameObject = GenerateColonistObject(position);
            Colonist colonist = colonistGameObject.AddComponent<Colonist>();

            colonist.Entity = new Data.Entity
            {
                GameObject = colonistGameObject,
                Speed = 0f,
                Position = new Vector2Int(position.x, position.y),
                Direction = Type.Direction.S,
            };

            colonist.Id = new Data.Id
            {
                FullName = NameGenerator.GetName(groupType),
                Number = population.NextId++,
                PopulationType = Type.Population.Citizen,
                GroupType = groupType,
            };

            colonistGameObject.name = colonist.Id.FullName;
            colonistGameObject.transform.parent = entitiesObject.transform;

            population.Colonists.Add(colonist);
        }


        private GameObject GenerateColonistObject(Vector2Int position)
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(position);

            GameObject colonistObject = Instantiate(
                colonistPrefab,
                new Vector3(worldPosition.x, worldPosition.y, 0),
                Quaternion.identity
            );

            return colonistObject;
        }


        public Data.Path RequestPath(Vector2Int start, Vector2Int end)
        {
            return pathfindingSystem.FindPath(start, end);
        }


        public void OnColonistBehaviorChange(string behaviorName)
        {
            if (behaviorName == "Wander Out")
            {
                print("Wander!");
            }
            else if (behaviorName == "Gather Home")
            {
                foreach (Colonist colonist in population.Colonists)
                {
                    colonist.Path = RequestPath(
                        colonist.Entity.Position,
                        mapSystem.GetColonyBase(colonist.Id.GroupType).Position
                    );
                }
            }
        }
    }
}

