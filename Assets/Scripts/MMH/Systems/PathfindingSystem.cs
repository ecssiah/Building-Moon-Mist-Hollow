using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingSystem : MonoBehaviour
{
    private MapSystem mapSystem;


    void Awake()
    {
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();
    }


    void Update()
    {
        
    }
}
