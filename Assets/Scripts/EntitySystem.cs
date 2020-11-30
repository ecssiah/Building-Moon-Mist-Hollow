using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    public GameObject characterPrefab;

    private NamingSystem namingSystem;


    void Start()
    {
        namingSystem = GetComponent<NamingSystem>();

        GenerateCharacter(new Vector3(-4, 0, 0));
        GenerateCharacter(new Vector3(4, 0, 0));
        GenerateCharacter(new Vector3(0, 4, 0));
        GenerateCharacter(new Vector3(0, -4, 0));
    }


    private void GenerateCharacter(Vector3 position)
    {
        GameObject characterObject = Instantiate(
            characterPrefab, position, Quaternion.identity
        );

        characterObject.transform.parent = this.transform;
        characterObject.name = namingSystem.GetName();
    }


    void Update()
    {
        
    }
}
