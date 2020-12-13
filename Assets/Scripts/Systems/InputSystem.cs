using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private MapSystem mapSystem;

    private SelectionHandler selectionHandler;


    void Awake()
    {
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        selectionHandler = gameObject.AddComponent<SelectionHandler>();
        selectionHandler.BroadcastCellSelection = OnCellSelection;
    }


    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionHandler.Select();
        }
    }


    public void OnCellSelection(Vector3Int cellPosition)
    {
        mapSystem.ClearSelection();


        mapSystem.SelectCell(cellPosition);
    }
}
