using System;
using System.Collections.Generic;
using Unity.Mathematics;
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
            mapSystem = GameObject.Find("Map System").GetComponent<MapSystem>();
            pathfindingSystem = GameObject.Find("Pathfinding System").GetComponent<PathfindingSystem>();

            population = new Data.Population
            {
                NextId = 0,
                Colonists = new List<Component.Colonist>(),
            };

            entitiesObject = GameObject.Find("Entities");
            colonistPrefab = Resources.Load<GameObject>("Prefabs/Colonist");
        }


        private void Start()
        {
            for (int i = 0; i < Info.Entity.NumberOfSeedColonists; i++)
            {
                Data.Cell cellData = mapSystem.Map.GetFreeCell();

                GenerateColonist(cellData.Position);
            }
        }


        private void GenerateColonist(int2 position)
        {
            Type.Group groupType = Util.Misc.RandomEnumValue<Type.Group>();

            GameObject colonistGameObject = GenerateColonistObject(position);
            Component.Colonist colonist = colonistGameObject.AddComponent<Component.Colonist>();

            colonist.Entity = new Data.Entity
            {
                GameObject = colonistGameObject,
                Speed = 0f,
                Position = position,
                Direction = Type.Direction.S,
            };

            colonist.Id = new Data.Id
            {
                FullName = Util.Entity.GetName(groupType),
                Number = population.NextId++,
                PopulationType = Type.Population.Citizen,
                GroupType = groupType,
            };

            colonist.Path = new Data.Path();

            colonistGameObject.name = colonist.Id.FullName;
            colonistGameObject.transform.parent = entitiesObject.transform;

            population.Colonists.Add(colonist);
        }


        private GameObject GenerateColonistObject(int2 position)
        {
            Vector2 worldPosition = Util.Map.IsoToWorld(position);

            GameObject colonistObject = Instantiate(
                colonistPrefab,
                new Vector3(worldPosition.x, worldPosition.y, 0),
                Quaternion.identity
            );

            return colonistObject;
        }


        public Data.Path RequestPath(int2 start, int2 end)
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
                foreach (Component.Colonist colonist in population.Colonists)
                {
                    colonist.Path = RequestPath(
                        colonist.Entity.Position,
                        mapSystem.Map.GetColonyBase(colonist.Id.GroupType).Position
                    );
                }
            }
        }
    }
}

