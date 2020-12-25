using UnityEngine;

public class UISystem : MonoBehaviour
{
    private EntitySystem entitySystem;
    private MapSystem mapSystem;

    private EntityLabeler entityLabeler;
    private InfoPanel infoPanel;


    void Awake()
    {
        entitySystem = GameObject.Find("EntitySystem").GetComponent<EntitySystem>();
        mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

        entityLabeler = gameObject.AddComponent<EntityLabeler>();
        infoPanel = gameObject.AddComponent<InfoPanel>();
    }


    public void SelectEntity(GameObject entity)
    {
        CitizenData entityData = entitySystem.GetCitizenData(entity);

        entityLabeler.SelectEntity(entity);

        infoPanel.ActivateEntityMode(entityData);
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
