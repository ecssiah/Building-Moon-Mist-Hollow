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
        uiSystem.ClearSelection();
        mapSystem.ClearSelection();

        uiSystem.SelectEntity(entity);
    }


    public void OnCellSelection(Vector3Int cellPosition)
    {
        uiSystem.ClearSelection();
        mapSystem.ClearSelection();

        uiSystem.SelectCell(cellPosition);
        mapSystem.SelectCell(cellPosition);
    }
}
