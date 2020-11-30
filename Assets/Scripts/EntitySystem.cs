using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    public GameObject characterPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameObject characterObject = Instantiate(
            characterPrefab, new Vector3(0, 0, 0), Quaternion.identity
        );

        characterObject.transform.parent = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
