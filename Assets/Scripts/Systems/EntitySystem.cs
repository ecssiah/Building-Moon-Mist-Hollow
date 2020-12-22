﻿using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private MapSystem mapSystem;

    private NameGenerator nameGenerator;

    private EntityData entityData;

    private GameObject citizenPrefab;

    void Awake()
    {
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        nameGenerator = gameObject.AddComponent<NameGenerator>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );
    }


    void Start()
    {
        for (int i = 0; i < EntityInfo.NumberOfSeedCitizens; i++)
        {
            Vector2Int position = MapUtil.GetRandomMapPosition();

            while (mapSystem.GetCellData(position).solid)
            {
                position = MapUtil.GetRandomMapPosition();
            }

            GenerateCitizen(position);
        }
    }


    private void GenerateCitizen(Vector2 position)
    {
        Vector2 worldPosition = MapUtil.IsoToWorld(position);

        GameObject newCitizenObject = Instantiate(
            citizenPrefab,
            new Vector3(worldPosition.x, worldPosition.y, 0),
            Quaternion.identity
        );

        newCitizenObject.transform.parent = transform;
        newCitizenObject.name = nameGenerator.GetName();
        newCitizenObject.layer = LayerMask.NameToLayer("Citizens");

        CitizenComponent newCitizen = newCitizenObject.AddComponent<CitizenComponent>();
        newCitizen.CitizenData = new CitizenData()
        {
            name = newCitizenObject.name,
            citizenNumber = entityData.nextCitizenNumber++,
            groupData = new GroupData { groupType = Util.RandomEnumValue<GroupType>() },
        };
    }


    public CitizenData GetCitizenData(GameObject citizenObject)
    {
        CitizenComponent citizenComponent = citizenObject.GetComponent<CitizenComponent>();

        return citizenComponent.CitizenData;
    }
}
