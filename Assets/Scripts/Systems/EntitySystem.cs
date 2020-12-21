﻿using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private MapSystem mapSystem;
    private NamingSystem namingSystem;

    private EntityData entityData;

    private GameObject citizenPrefab;

    private List<CitizenData> entityDataList;


    void Awake()
    {
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        namingSystem = GetComponent<NamingSystem>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );

        entityDataList = new List<CitizenData>();
    }


    void Start()
    {
        for (int i = 0; i < 10; i++)
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

        GameObject newCharacterObject = Instantiate(
            citizenPrefab,
            new Vector3(worldPosition.x, worldPosition.y, 0),
            Quaternion.identity
        );

        newCharacterObject.transform.parent = transform;

        newCharacterObject.name = namingSystem.GetName();
        newCharacterObject.layer = LayerMask.NameToLayer("Citizens");

        newCharacterObject.AddComponent<CitizenMovement>();
        newCharacterObject.AddComponent<CitizenAnimator>();

        CitizenData citizenData = new CitizenData()
        {
            entity = newCharacterObject,
            name = newCharacterObject.name,
            citizenNumber = entityData.nextCitizenNumber++,
        };

        entityDataList.Add(citizenData);
    }


    public CitizenData GetEntityData(GameObject entity)
    {
        return entityDataList.Find(entityData => entityData.entity == entity);
    }
}
