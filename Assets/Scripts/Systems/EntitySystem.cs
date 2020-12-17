using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private NamingSystem namingSystem;

    private GameObject citizenPrefab;

    private List<EntityData> entityDataList;


    void Awake()
    {
        namingSystem = GetComponent<NamingSystem>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );

        entityDataList = new List<EntityData>();

        GenerateCitizen(new Vector2( 2, 2));
        GenerateCitizen(new Vector2(-2, 2));
        GenerateCitizen(new Vector2( 2, -2));
        GenerateCitizen(new Vector2(-2, -2));

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

        EntityData entityData = new EntityData()
        {
            entity = newCharacterObject,
            name = newCharacterObject.name,
        };

        entityDataList.Add(entityData);
    }


    public EntityData GetEntityData(GameObject entity)
    {
        return entityDataList.Find(entityData => entityData.entity == entity);
    }
}
