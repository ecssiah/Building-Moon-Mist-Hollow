using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private EntityData entityData;
    private CellData cellData;

    private InfoMode mode;

    private TextMeshProUGUI entityNameText;
    private TextMeshProUGUI cellPositionText;

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


    void Awake()
    {
        entityTab = GameObject.Find("Entity Tab");
        cellTab = GameObject.Find("Cell Tab");

        Transform entityNameTransform = entityTab.transform.Find("Name");
        Transform cellPositionTransform = cellTab.transform.Find("Position");

        entityNameText = entityNameTransform.Find("Data").GetComponent<TextMeshProUGUI>();
        cellPositionText = cellPositionTransform.Find("Data").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI entityNameLabel = entityNameTransform.Find("Label").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cellPositionLabel = cellPositionTransform.Find("Label").GetComponent<TextMeshProUGUI>();

        entityNameLabel.text = "Name: ";
        cellPositionLabel.text = "Position: ";
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


    public void ActivateEntityMode(EntityData entityData = new EntityData())
    {
        entityTab.SetActive(true);
        cellTab.SetActive(false);

        entityNameText.text = entityData.name;
    }


    public void ActivateCellMode(CellData cellData = new CellData())
    {
        entityTab.SetActive(false);
        cellTab.SetActive(true);

        cellPositionText.text = $"{cellData.position}";
    }


    public void Deactivate()
    {
        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
