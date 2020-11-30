using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionHandler : MonoBehaviour
{
    private SelectionData selectionData;
    private Tile selectionTile;
    private Dictionary<string, Tilemap> tilemaps;

    private InfoPanelController infoPanelController;


    void Start()
    {
        InitTilemaps();
        InitInfoPanel();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnSelection();
        }
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


    private void InitInfoPanel()
    {
        infoPanelController = GameObject.Find("Info Panel").GetComponent<InfoPanelController>();
    }


    private void OnEntitySelection(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
    }


    private void OnSelection()
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


    private void OnTilemapSelection()
    {
        ResetSelection();

        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int isoGridVector = Utilities.ToIsoGrid(selectedPosition);

        tilemaps["Overlay"].SetTile(isoGridVector, selectionTile);

        infoPanelController.updateSelection(isoGridVector);

        selectionData.selectedCell = isoGridVector;
    }


    private void ResetSelection()
    {
        tilemaps["Overlay"].SetTile(selectionData.selectedCell, null);
    }
}
