using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private NamingSystem namingSystem;

    private GameObject citizenPrefab;

    private List<EntityData> entityDataList;


    void Awake()
    {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );

        namingSystem = GetComponent<NamingSystem>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        entityDataList = new List<EntityData>();

        GenerateCitizen(new Vector2Int(-2, 2));
        GenerateCitizen(new Vector2Int(2, -2));
        GenerateCitizen(new Vector2Int(2, 2));
        GenerateCitizen(new Vector2Int(-2, -2));
    }


    private void GenerateCitizen(Vector2Int cellPosition)
    {
        Vector3 worldPosition = MapUtil.IsoToWorld(cellPosition);

        GameObject newCitizenObject = Instantiate(
            citizenPrefab,
            new Vector3(worldPosition.x, worldPosition.y, 0),
            Quaternion.identity
        );

        newCitizenObject.transform.parent = transform;

        newCitizenObject.name = namingSystem.GetName();
        newCitizenObject.layer = LayerMask.NameToLayer("Citizens");

        newCitizenObject.AddComponent<CitizenMovement>();
        newCitizenObject.AddComponent<CitizenAnimator>();

        EntityData entityData = new EntityData()
        {
            entity = newCitizenObject,
            name = newCitizenObject.name,
        };

        entityDataList.Add(entityData);
    }


    public EntityData GetEntityData(GameObject entity)
    {
        return entityDataList.Find(entityData => entityData.entity == entity);
    }
}
