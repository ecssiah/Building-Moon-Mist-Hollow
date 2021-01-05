using UnityEngine;

public class UISystem : MonoBehaviour
{
    private Map map;

    private EntityLabeler entityLabeler;
    private InfoPanel infoPanel;


    void Awake()
    {
        map = GameObject.Find("MapSystem").GetComponent<Map>();

        entityLabeler = gameObject.AddComponent<EntityLabeler>();
        infoPanel = gameObject.AddComponent<InfoPanel>();
    }


    public void SelectEntity(GameObject entity)
    {
        Citizen citizen = entity.GetComponent<Citizen>();

        entityLabeler.SelectEntity(entity);

        infoPanel.DisplayCitizen(citizen);
    }


    public void SelectCell(Vector2Int cellPosition)
    {
        CellData cellData = map.GetCell(cellPosition);

        infoPanel.ActivateCellMode(cellData);
    }


    public void ClearSelection()
    {
        entityLabeler.Clear();

        infoPanel.Deactivate();
    }
}
