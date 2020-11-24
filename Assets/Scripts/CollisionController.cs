using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    public bool showCollision = false;


    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapRenderer.enabled = showCollision;
    }


    void Update()
    {
        
    }
}
