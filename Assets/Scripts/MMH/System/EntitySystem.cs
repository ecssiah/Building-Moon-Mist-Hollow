using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH.System
{
    public class EntitySystem : MonoBehaviour, Handler.IEntityEventHandler
    {
        private MapSystem mapSystem;

        private Data.Population population;

        private GameObject entitiesObject;
        private GameObject colonistPrefab;


        private void Awake()
        {
            mapSystem = GameObject.Find("Map System").GetComponent<MapSystem>();

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

            colonist.Entity.GameObject = colonistGameObject;
            colonist.Entity.Position = position;

            colonist.Id.Number = population.NextId++;
            colonist.Id.PopulationType = Type.Population.Citizen;
            colonist.Id.GroupType = groupType;
            colonist.Id.FullName = Util.Entity.GetName(groupType);

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


        public void SetColonistBehavior(Type.Behavior behavior)
        {
            foreach (Component.Colonist colonist in population.Colonists)
            {
                colonist.Behavior = behavior;

                if (behavior == Type.Behavior.Gather)
                {
                    colonist.Behavior = Type.Behavior.Gather;

                    Data.ColonyBase colonyBase = mapSystem.Map.GetColonyBase(colonist.Id.GroupType);

                    colonist.FindPath(colonyBase.Position);

                    print("Gather");
                    print(colonist.Path);
                }
            }
        }
    }
}

