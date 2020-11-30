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

        GameObject characterObject1 = Instantiate(
            characterPrefab, new Vector3(0, 0, 0), Quaternion.identity
        );

        characterObject1.transform.parent = this.transform;
        characterObject1.name = namingSystem.names[0];

        GameObject characterObject2 = Instantiate(
            characterPrefab, new Vector3(4, 4, 0), Quaternion.identity
        );

        characterObject2.transform.parent = this.transform;
        characterObject2.name = namingSystem.names[1];

        GameObject characterObject3 = Instantiate(
            characterPrefab, new Vector3(-4, -4, 0), Quaternion.identity
        );

        characterObject3.transform.parent = this.transform;
        characterObject3.name = namingSystem.names[2];
    }


    void Update()
    {
        
    }
}
