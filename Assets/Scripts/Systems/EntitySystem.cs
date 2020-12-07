using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    public GameObject CharacterPrefab;

    private NamingSystem NamingSystem;


    void Awake()
    {
        Physics2D.IgnoreLayerCollision(8, 8, true);

        NamingSystem = GetComponent<NamingSystem>();

        GenerateCharacter(new Vector3(-2, 2, 0));
        GenerateCharacter(new Vector3(-2, -2, 0));
        GenerateCharacter(new Vector3(2, -2, 0));
        GenerateCharacter(new Vector3(2, 2, 0));
    }


    void Start()
    {

    }


    void Update()
    {
        
    }


    private void GenerateCharacter(Vector3 position)
    {
        Vector3 ScreenPosition = Utilities.CartesianToIso(position * 0.5f);

        GameObject CharacterObject = Instantiate(
            CharacterPrefab, ScreenPosition, Quaternion.identity
        );

        CharacterObject.transform.parent = this.transform;
        CharacterObject.name = NamingSystem.GetName();
        CharacterObject.layer = 8;
    }
}
