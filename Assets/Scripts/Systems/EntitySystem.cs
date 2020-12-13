using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private NamingSystem namingSystem;

    private GameObject citizenPrefab;


    void Awake()
    {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );

        namingSystem = GetComponent<NamingSystem>();

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");

        GenerateCitizen(new Vector3Int(-2, 2, 0));
        GenerateCitizen(new Vector3Int(2, -2, 0));
        GenerateCitizen(new Vector3Int(2, 2, 0));
        GenerateCitizen(new Vector3Int(-2, -2, 0));
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }



    private void GenerateCitizen(Vector3Int cellPosition)
    {
        Vector3 worldPosition = Utilities.IsoToWorld(cellPosition);

        GameObject newCitizenObject = Instantiate(
            citizenPrefab, worldPosition, Quaternion.identity
        );

        newCitizenObject.transform.parent = this.transform;
        newCitizenObject.name = namingSystem.GetName();
        newCitizenObject.layer = LayerMask.NameToLayer("Citizens");
    }
}
