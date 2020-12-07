using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionHandler : MonoBehaviour
{
    private Tile selectedTile;
    private Vector3Int currentCell;
    private Dictionary<string, Tilemap> tilemaps;

    private InfoPanelController infoPanelController;
    private GameObject cellInfoObject;
    private GameObject entityInfoObject;


    void Awake()
    {
        InitTilemaps();
        GatherComponents();
    }


    private void GatherComponents()
    {
        cellInfoObject = GameObject.Find("Cell");
        entityInfoObject = GameObject.Find("Entity");

        infoPanelController = GameObject.Find("Info").GetComponent<InfoPanelController>();
    }


    void Start()
    {
        
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
        tilemaps = new Dictionary<string, Tilemap>();
        selectedTile = Resources.Load<Tile>("Tiles/Selection_1");

        Tilemap[] tilemapsArray = GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapsArray)
        {
            tilemaps[tilemap.name] = tilemap;
        }
    }


    private void OnEntitySelection(GameObject gameObject)
    {
        cellInfoObject.SetActive(false);
        entityInfoObject.SetActive(true);

        ResetSelection();
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
        cellInfoObject.SetActive(true);
        entityInfoObject.SetActive(false);

        ResetSelection();

        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectedPosition.y += 0.25f;

        currentCell = Utilities.ScreenToIsoGrid(selectedPosition);

        tilemaps["Overlay"].SetTile(currentCell, selectedTile);
        infoPanelController.UpdateSelection(currentCell);
    }


    private void ResetSelection()
    {
        tilemaps["Overlay"].SetTile(currentCell, null);
    }
}
