using UnityEngine;

public class UISystem : MonoBehaviour
{
    private EntityLabeler entityLabeler;
    private InfoPanel infoPanel;


    void Awake()
    {
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


    public void SelectCell(Vector3Int cellPosition)
    {
        CellData cellData = new CellData
        {
            position = cellPosition
        };

        infoPanel.ActivateCellMode(cellData);
    }


    public void ClearSelection()
    {
        entityLabeler.Clear();
        infoPanel.Deactivate();
    }
}
