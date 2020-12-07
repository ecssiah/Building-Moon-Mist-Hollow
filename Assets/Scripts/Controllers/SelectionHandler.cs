using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionHandler : MonoBehaviour
{
    private Tile selectedTile;
    private Vector3Int currentCell;
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
        selectedTile = Resources.Load<Tile>("Tiles/Selection_1");

        Tilemap[] tilemapsArray = GameObject.Find("Grid").GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }
    }


    public void Select()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            OnEntitySelection(hit.transform.gameObject);
        }
        else
        {
            OnTilemapSelection();
        }
    }


    private void OnEntitySelection(GameObject gameObject)
    {
        ResetSelection();

        Debug.Log(gameObject.name);
    }


    private void OnTilemapSelection()
    {
        ResetSelection();

        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedPosition.y += 0.25f;

        currentCell = Utilities.ScreenToIsoGrid(selectedPosition);

        tilemaps["Overlay"].SetTile(currentCell, selectedTile);
    }


    private void ResetSelection()
    {
        tilemaps["Overlay"].SetTile(currentCell, null);
    }
}
