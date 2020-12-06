using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionHandler : MonoBehaviour
{
    private SelectionData SelectionData;
    private Tile SelectedTile;
    private Dictionary<string, Tilemap> Tilemaps;

    private InfoPanelController InfoPanelController;


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
        SelectionData = GetComponentInParent<SelectionData>();
        SelectedTile = Resources.Load<Tile>("Tiles/Selection_1");

        Tilemaps = new Dictionary<string, Tilemap>();

        Tilemap[] TilemapsArray = GetComponentsInChildren<Tilemap>();

        foreach (Tilemap Tilemap in TilemapsArray)
        {
            Tilemaps[Tilemap.name] = Tilemap;
        }
    }


    private void InitInfoPanel()
    {
        InfoPanelController = GameObject.Find("Info Panel").GetComponent<InfoPanelController>();
    }


    private void OnEntitySelection(GameObject gameObject)
    {
        Debug.Log(gameObject.name);
    }


    private void OnSelection()
    {
        Ray CameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D Hit = Physics2D.Raycast(CameraRay.origin, CameraRay.direction);

        if (Hit.collider != null)
        {
            OnEntitySelection(Hit.transform.gameObject);
        }
        else
        {
            OnTilemapSelection();
        }
    }


    private void OnTilemapSelection()
    {
        ResetSelection();

        Vector3 SelectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SelectedPosition.y += 0.25f;

        Vector3Int IsoGridVector = Utilities.ScreenToIsoGrid(SelectedPosition);

        Tilemaps["Overlay"].SetTile(IsoGridVector, SelectedTile);

        InfoPanelController.updateSelection(IsoGridVector);

        SelectionData.selectedCell = IsoGridVector;
    }


    private void ResetSelection()
    {
        Tilemaps["Overlay"].SetTile(SelectionData.selectedCell, null);
    }
}
