using UnityEngine;
using System.Collections;

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
        entitySystem.SelectEntity(entity);
    }


    public void OnCellSelection(Vector3Int cellPosition)
    {
        uiSystem.SelectCell(cellPosition);
        mapSystem.SelectCell(cellPosition);
    }
}
