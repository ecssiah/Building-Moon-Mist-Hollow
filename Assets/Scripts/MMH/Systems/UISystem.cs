using UnityEngine;

public class UISystem : MonoBehaviour
{
    private WorldMap worldMap;

    private EntityLabeler entityLabeler;
    private InfoPanel infoPanel;


    void Awake()
    {
        entityLabeler = gameObject.AddComponent<EntityLabeler>();
        infoPanel = gameObject.AddComponent<InfoPanel>();
    }


    void Start()
    {
        worldMap = GameObject.Find("MapSystem").GetComponent<WorldMap>();
    }


    public void SelectEntity(GameObject entity)
    {
        Citizen citizen = entity.GetComponent<Citizen>();

        entityLabeler.SelectEntity(entity);

        infoPanel.DisplayCitizen(citizen);
    }


    public void SelectCell(Vector2Int cellPosition)
    {
        CellData cellData = worldMap.GetCell(cellPosition);

        infoPanel.ActivateCellMode(cellData);
    }


    public void ClearSelection()
    {
        entityLabeler.Clear();

        infoPanel.Deactivate();
    }
}
