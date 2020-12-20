using UnityEngine;
using TMPro;
using System;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private TextMeshProUGUI entityNameText;

    private TextMeshProUGUI cellPositionText;
    private TextMeshProUGUI cellTypeText;
    private TextMeshProUGUI cellBuildingTypeText;

    private InfoType mode;
    public InfoType Mode { get => mode; }

    private EntityData entityData;
    public EntityData EntityData { get => entityData; }

    private CellData cellData;
    public CellData CellData { get => cellData; }


    void Awake()
    {
        SetupEntityTab();
        SetupCellTab();

        mode = InfoType.None;
    }


    private void SetupEntityTab()
    {
        entityTab = GameObject.Find("Entity Tab");
        entityTab.SetActive(false);

        entityNameText = UIUtil.SetLabel(
            "Name", entityTab.transform.Find("Name")
        );
    }


    private void SetupCellTab()
    {
        cellTab = GameObject.Find("Cell Tab");
        cellTab.SetActive(false);

        cellPositionText = UIUtil.SetLabel(
            "Position", cellTab.transform.Find("Position")
        );
        cellTypeText = UIUtil.SetLabel(
            "Cell", cellTab.transform.Find("Cell Type")
        );
        cellBuildingTypeText = UIUtil.SetLabel(
            "Building", cellTab.transform.Find("Building Type")
        );
    }


    public void ActivateEntityMode(EntityData entityData)
    {
        mode = InfoType.Entity;

        entityTab.SetActive(true);
        cellTab.SetActive(false);

        entityNameText.text = entityData.name;
    }


    public void ActivateCellMode(CellData cellData)
    {
        mode = InfoType.Cell;

        entityTab.SetActive(false);
        cellTab.SetActive(true);

        cellPositionText.text = $"{cellData.position}";
        cellTypeText.text = $"{Enum.GetName(typeof(GroundType), cellData.groundType)}";
        cellBuildingTypeText.text = $"{Enum.GetName(typeof(BuildingType), cellData.buildingType)}";
    }


    public void Deactivate()
    {
        mode = InfoType.None;

        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
