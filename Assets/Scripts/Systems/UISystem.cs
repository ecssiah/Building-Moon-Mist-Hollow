using UnityEngine;

public class UISystem : MonoBehaviour
{
    private EntitySystem entitySystem;
    private MapSystem mapSystem;

    private InfoPanel infoPanel;
    private EntityLabeler entityLabeler;


    void Awake()
    {
        entitySystem = GameObject.Find("Entities").GetComponent<EntitySystem>();
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        infoPanel = gameObject.AddComponent<InfoPanel>();
        entityLabeler = gameObject.AddComponent<EntityLabeler>();
    }


    public void SelectEntity(GameObject entity)
    {
        EntityData entityData = entitySystem.GetEntityData(entity);

        infoPanel.ActivateEntityMode(entityData);

        entityLabeler.SelectEntity(entity);
    }


    public void SelectCell(Vector2Int cellPosition)
    {
        CellData cellData = mapSystem.GetCellData(cellPosition.x, cellPosition.y);

        infoPanel.ActivateCellMode(cellData);
    }


    public void ClearSelection()
    {
        infoPanel.Deactivate();
        entityLabeler.ClearSelection();
    }
}
