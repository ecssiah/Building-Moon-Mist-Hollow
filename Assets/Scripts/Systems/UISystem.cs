using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    private EntitySystem entitySystem;

    private EntityLabeler entityLabeler;
    private InfoPanel infoPanel;


    void Awake()
    {
        entitySystem = GameObject.Find("Entities").GetComponent<EntitySystem>();

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
        Debug.Log(entity);
    }


    public void SelectCell(Vector3Int cellPosition)
    {
        Debug.Log(cellPosition);
    }
}
