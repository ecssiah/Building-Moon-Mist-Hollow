using UnityEngine;
using System.Collections;

public class InputSystem : MonoBehaviour
{
    private MapSystem mapSystem;
    private UISystem uiSystem;

    private SelectionHandler selectionHandler;


    void Awake()
    {
        uiSystem = GameObject.Find("UI").GetComponent<UISystem>();
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();
    }


    void Start()
    {
        selectionHandler = gameObject.AddComponent<SelectionHandler>();
        selectionHandler.BroadcastEntitySelection = OnEntitySelection;
        selectionHandler.BroadcastCellSelection = OnCellSelection;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionHandler.Select();
        }
    }


    public void OnEntitySelection(GameObject entity)
    {
        uiSystem.SelectEntity(entity);
    }


    public void OnCellSelection(Vector3Int cellPosition)
    {
        uiSystem.SelectCell(cellPosition);
        mapSystem.SelectCell(cellPosition);
    }
}
