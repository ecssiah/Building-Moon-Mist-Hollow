using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionHandler : MonoBehaviour
{
    private SelectionData selectionData;
    private Tile selectionTile;

    private Dictionary<string, Tilemap> tilemaps;


    void Start()
    {
        InitTilemaps();
    }


    private void InitTilemaps()
    {
        selectionData = GetComponentInParent<SelectionData>();

        selectionTile = Resources.Load<Tile>("Tiles/Selection_1");

        tilemaps = new Dictionary<string, Tilemap>();

        Tilemap[] tilemapsArray = GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }


    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                OnEntitySelection(hit.transform.gameObject);
            } else
            {
                OnTilemapSelection();
            }
        }
    }


    private void OnEntitySelection(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
    }


    private void OnTilemapSelection()
    {
        tilemaps["Overlay"].SetTile(selectionData.selectedCell, null);

        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int isoGridVector = Utilities.ToIsoGrid(selectedPosition);

        tilemaps["Overlay"].SetTile(isoGridVector, selectionTile);

        selectionData.selectedCell = isoGridVector;
    }
}
