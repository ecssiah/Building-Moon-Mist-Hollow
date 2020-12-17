using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private UISystem uiSystem;
    private MapSystem mapSystem;
    private EntitySystem entitySystem;

    private SelectionHandler selectionHandler;


    void Awake()
    {
        uiSystem = GameObject.Find("UI").GetComponent<UISystem>();
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();
        entitySystem = GameObject.Find("Entities").GetComponent<EntitySystem>();

        selectionHandler = gameObject.AddComponent<SelectionHandler>();
        selectionHandler.BroadcastCellSelection = OnCellSelection;
        selectionHandler.BroadcastEntitySelection = OnEntitySelection;
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


    public void OnCellSelection(Vector2Int cellPosition)
    {
        uiSystem.ClearSelection();
        mapSystem.ClearSelection();

        uiSystem.SelectCell(cellPosition);
        mapSystem.SelectCell(cellPosition);
    }


    public void OnEntitySelection(GameObject entity)
    {
        uiSystem.ClearSelection();
        mapSystem.ClearSelection();

        uiSystem.SelectEntity(entity);
    }
}
