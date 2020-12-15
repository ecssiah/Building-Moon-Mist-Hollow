using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private InfoMode mode;
    
    private EntityData entityData;
    private CellData cellData;

    public InfoMode Mode
    {
        get => mode;

        set
        {
            mode = value;
            UpdateMode();
        }
    }

    public EntityData EntityData { get => entityData; set => entityData = value; }
    public CellData CellData { get => cellData; set => cellData = value; }

    private TextMeshProUGUI entityNameText;
    private TextMeshProUGUI cellPositionText;
    

    void Awake()
    {
        entityTab = GameObject.Find("Entity Tab");
        cellTab = GameObject.Find("Cell Tab");

        Transform entityNameTransform = entityTab.transform.Find("Name");
        Transform cellPositionTransform = cellTab.transform.Find("Position");

        entityNameText = entityNameTransform
            .Find("Data")
            .GetComponent<TextMeshProUGUI>();

        cellPositionText = cellPositionTransform
            .Find("Data")
            .GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI entityNameLabel = entityNameTransform
            .Find("Label")
            .GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI cellPositionLabel = cellPositionTransform
            .Find("Label")
            .GetComponent<TextMeshProUGUI>();

        entityNameLabel.text = "Name: ";
        cellPositionLabel.text = "Position: ";

        Mode = InfoMode.None;
    }


    void Start()
    {
        
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

        entityNameText.text = entityData.name;
    }


    private void ActivateCellMode()
    {
        entityTab.SetActive(false);
        cellTab.SetActive(true);

        cellPositionText.text = $"{cellData.position}";
    }


    private void Deactivate()
    {
        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
