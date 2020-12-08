using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    private EntitySystem entitySystem;

    private EntityLabeler entityLabeler;


    void Awake()
    {
        entitySystem = GameObject.Find("Entities").GetComponent<EntitySystem>();

        entityLabeler = this.gameObject.AddComponent<EntityLabeler>();
    }


    void Start()
    {
        entityLabeler.SetEntities(entitySystem.GetEntities());
    }


    void Update()
    {
        
    }
}
