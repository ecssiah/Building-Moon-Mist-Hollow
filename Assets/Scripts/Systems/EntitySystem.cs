using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private GameObject characterPrefab;

    private GameObject[] entities;

    private NamingSystem namingSystem;


    void Awake()
    {
        Physics2D.IgnoreLayerCollision(
            LayerMask.NameToLayer("Characters"),
            LayerMask.NameToLayer("Characters"),
            true
        );

        characterPrefab = Resources.Load<GameObject>("Prefabs/Character");
        namingSystem = GetComponent<NamingSystem>();

        GenerateCharacter(new Vector3(-2, 2, 0));
        GenerateCharacter(new Vector3(-2, -2, 0));
        GenerateCharacter(new Vector3(2, -2, 0));
        GenerateCharacter(new Vector3(2, 2, 0));
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


    private void GenerateCharacter(Vector3 position)
    {
        Vector3 screenPosition = Utilities.CartesianToIso(position * 0.5f);

        GameObject newCharacterObject = Instantiate(
            characterPrefab, screenPosition, Quaternion.identity
        );

        newCharacterObject.transform.parent = this.transform;
        newCharacterObject.name = namingSystem.GetName();
        newCharacterObject.layer = LayerMask.NameToLayer("Characters");
    }

         
    public GameObject[] GetEntities()
    {
        return entities;
    }


    public void SelectEntity(GameObject entity)
    {
        Debug.Log(entity);
    }
}
