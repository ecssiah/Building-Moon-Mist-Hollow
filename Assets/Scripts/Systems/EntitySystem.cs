using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private GameObject citizenPrefab;

    private GameObject[] entities;

    private NamingSystem namingSystem;


    void Awake()
    {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Citizens"),
            LayerMask.NameToLayer("Citizens"),
            true
        );

        citizenPrefab = Resources.Load<GameObject>("Prefabs/Citizen");
        namingSystem = GetComponent<NamingSystem>();

        GenerateCitizen(new Vector3(-2, 2, 0));
        GenerateCitizen(new Vector3(-2, -2, 0));
        GenerateCitizen(new Vector3(2, -2, 0));
        GenerateCitizen(new Vector3(2, 2, 0));
    }


    void Start()
    {
        entities = new GameObject[this.transform.childCount];

        int index = 0;
        foreach (Transform transform in this.transform)
        {
            entities[index++] = transform.gameObject;
        }
    }


    void Update()
    {
        
    }


    private void GenerateCitizen(Vector3 position)
    {
        Vector3 worldPosition = MapUtil.IsoToWorld(position);

        GameObject newCharacterObject = Instantiate(
            citizenPrefab, worldPosition, Quaternion.identity
        );

        newCharacterObject.transform.parent = this.transform;
        newCharacterObject.name = namingSystem.GetName();
        newCharacterObject.layer = LayerMask.NameToLayer("Citizens");
    }

         
    public GameObject[] GetEntities()
    {
        return entities;
    }


    public void SelectEntity(GameObject entity)
    {
        Debug.Log(entity);
    }


    public void ClearSelection()
    {

    }
}
