using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{

    private SelectionHandler selectionHandler;


    void Awake()
    {
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
        Debug.Log(cellPosition);
    }
}
