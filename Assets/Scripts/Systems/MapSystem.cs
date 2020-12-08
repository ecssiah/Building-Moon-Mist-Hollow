using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapSystem: MonoBehaviour
{
    private MapData map;

    private Dictionary<string, Tilemap> tilemaps;



    void Awake()
    {

    }


    void Start()
    {
        InitTilemaps();
    }


    void Update()
    {
        
    }


    private void InitTilemaps()
    {
        tilemaps = new Dictionary<string, Tilemap>();

        Tilemap[] tilemapsArray = GameObject.Find("Map").GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }
    }


    public TileData GetTileData(int x, int y)
    {
        return new TileData();
    }

}
