using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    private EntitySystem entitySystem;
    private MapSystem mapSystem;

    private EntityLabeler entityLabeler;
    private InfoPanel infoPanel;


    void Awake()
    {
        entitySystem = GameObject.Find("Entities").GetComponent<EntitySystem>();
        mapSystem = GameObject.Find("Map").GetComponent<MapSystem>();

        entityLabeler = this.gameObject.AddComponent<EntityLabeler>();
        infoPanel = this.gameObject.AddComponent<InfoPanel>();
    }


    void Start()
    {
        entityLabeler.Entities = entitySystem.GetEntities();
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

        infoPanel.Mode = InfoMode.Entity;
        infoPanel.ActivateEntityMode(entityData);
    }


    public void SelectCell(Vector3Int cellPosition)
    {
        CellData cellData = new CellData
        {
            position = cellPosition
        };

        infoPanel.Mode = InfoMode.Cell;
        infoPanel.ActivateCellMode(cellData);
    }
}
