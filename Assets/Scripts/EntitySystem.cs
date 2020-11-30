using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    public GameObject characterPrefab;

    private NamingSystem namingSystem;


    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 8, true);

        namingSystem = GetComponent<NamingSystem>();

        GenerateCharacter(new Vector3(-1, 0, 0));
        GenerateCharacter(new Vector3(1, 0, 0));
        GenerateCharacter(new Vector3(0, 1, 0));
        GenerateCharacter(new Vector3(0, -1, 0));
    }


    private void GenerateCharacter(Vector3 position)
    {
        GameObject characterObject = Instantiate(
            characterPrefab, position, Quaternion.identity
        );

        characterObject.transform.parent = this.transform;
        characterObject.name = namingSystem.GetName();
        characterObject.layer = 8;
    }


    void Update()
    {
        
    }
}
