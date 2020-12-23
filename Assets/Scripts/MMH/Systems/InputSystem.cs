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
        selectionHandler.BroadcastEntitySelection = OnEntitySelection;
        selectionHandler.BroadcastCellSelection = OnCellSelection;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            selectionHandler.Select();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiSystem.ClearSelection();
            mapSystem.ClearSelection();
        }
    }


    public void OnEntitySelection(GameObject entity)
    {
        uiSystem.ClearSelection();
        mapSystem.ClearSelection();

        uiSystem.SelectEntity(entity);
    }


    public void OnCellSelection(Vector2Int cellPosition)
    {
        uiSystem.ClearSelection();
        mapSystem.ClearSelection();

        uiSystem.SelectCell(cellPosition);
        mapSystem.SelectCell(cellPosition);
    }
}
