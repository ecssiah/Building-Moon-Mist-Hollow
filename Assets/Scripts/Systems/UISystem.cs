using UnityEngine;

public class UISystem : MonoBehaviour
{
    private MapSystem mapSystem;

    private EntityLabeler entityLabeler;
    private InfoPanel infoPanel;


    void Awake()
    {
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        entityLabeler = gameObject.AddComponent<EntityLabeler>();
        infoPanel = gameObject.AddComponent<InfoPanel>();
    }


    public void SelectEntity(GameObject entity)
    {
        EntityData entityData = new EntityData
        {
            entity = entity,
            name = entity.name
        };

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
        entityLabeler.Clear();
        infoPanel.Deactivate();
    }
}
