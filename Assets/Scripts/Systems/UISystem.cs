using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    private EntitySystem entitySystem;
    private MapSystem mapSystem;

    private InfoPanel infoPanel;


    void Awake()
    {
        entitySystem = GameObject.Find("Entities").GetComponent<EntitySystem>();
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        infoPanel = this.gameObject.AddComponent<InfoPanel>();
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
