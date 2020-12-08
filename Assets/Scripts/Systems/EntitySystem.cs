using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private GameObject characterPrefab;

    private NamingSystem namingSystem;


    void Awake()
    {
        Physics2D.IgnoreLayerCollision(8, 8, true);

        characterPrefab = Resources.Load<GameObject>("Prefabs/Character");
        namingSystem = GetComponent<NamingSystem>();
    }


    void Start()
    {
        GenerateCharacter(new Vector3(-2, 2, 0));
        GenerateCharacter(new Vector3(-2, -2, 0));
        GenerateCharacter(new Vector3(2, -2, 0));
        GenerateCharacter(new Vector3(2, 2, 0));
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
}
