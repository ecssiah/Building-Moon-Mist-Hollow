using System.Collections;
using System.Collections.Generic;
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

        infoPanel = this.gameObject.AddComponent<InfoPanel>();
        entityLabeler = this.gameObject.AddComponent<EntityLabeler>();
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }


    public void SelectEntity(GameObject entity)
    {
        EntityData entityData = new EntityData
        {
            name = entity.name
        };

        infoPanel.ActivateEntityMode(entityData);

        entityLabeler.SelectEntity(entity);
    }


    public void SelectCell(Vector2Int cellPosition)
    {
        CellData cellData = new CellData
        {
            position = cellPosition
        };

        infoPanel.ActivateCellMode(cellData);
    }


    public void ClearSelection()
    {
        infoPanel.Deactivate();
        entityLabeler.ClearSelection();
    }
}
