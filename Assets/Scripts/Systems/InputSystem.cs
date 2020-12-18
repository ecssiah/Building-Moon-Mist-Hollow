using UnityEngine;

public class InputSystem : MonoBehaviour
{
    private UISystem uiSystem;
    private MapSystem mapSystem;

    private SelectionHandler selectionHandler;


    void Awake()
    {
        uiSystem = GameObject.Find("UI").GetComponent<UISystem>();
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        selectionHandler = gameObject.AddComponent<SelectionHandler>();
        selectionHandler.BroadcastCellSelection = OnCellSelection;
        selectionHandler.BroadcastEntitySelection = OnEntitySelection;
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
