using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private EntityData entity;
    private CellData cell;

    private InfoMode mode;

    private TextMeshProUGUI entityNameText;

    public InfoMode Mode
    {
        get => mode;

        set
        {
            mode = value;
            UpdateMode();
        }
    }

    public EntityData Entity { get => entity; set => entity = value; }
    public CellData Cell { get => cell; set => cell = value; }

    void Awake()
    {
        entityTab = GameObject.Find("Entity Tab");
        cellTab = GameObject.Find("Cell Tab");

        entityNameText = entityTab.transform.Find("Name").Find("Data").GetComponent<TextMeshProUGUI>();
    }


    void Start()
    {
        Mode = InfoMode.None;
    }


    void Update()
    {
        
    }


    private void UpdateMode()
    {
        switch (mode)
        {
            case InfoMode.None:
                Deactivate();
                break;
            case InfoMode.Entity:
                ActivateEntityMode();
                break;
            case InfoMode.Cell:
                ActivateCellMode();
                break;
            default:
                Deactivate();
                break;
        }
    }


    private void ActivateEntityMode()
    {
        entityTab.SetActive(true);
        cellTab.SetActive(false);

        entityNameText.text = entity.name;
    }


    private void ActivateCellMode()
    {
        entityTab.SetActive(false);
        cellTab.SetActive(true);
    }


    private void Deactivate()
    {
        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
