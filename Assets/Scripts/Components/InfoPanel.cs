using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private EntityData entityData;
    public EntityData EntityData { get => entityData; set => entityData = value; }

    private CellData cellData;
    public CellData CellData { get => cellData; set => cellData = value; }

    private InfoMode mode;
    public InfoMode Mode { get => mode; }

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
    }


    void Start()
    {
        Deactivate();
    }


    void Update()
    {
        
    }


    public void ActivateEntityMode(EntityData entityData)
    {
        mode = InfoMode.Entity;

        entityTab.SetActive(true);
        cellTab.SetActive(false);

        entityNameText.text = entityData.name;
    }


    public void ActivateCellMode(CellData cellData)
    {
        mode = InfoMode.Cell;

        entityTab.SetActive(false);
        cellTab.SetActive(true);

        cellPositionText.text = $"{cellData.position}";
    }


    public void Deactivate()
    {
        mode = InfoMode.None;

        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
