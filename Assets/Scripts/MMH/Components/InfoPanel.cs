using UnityEngine;
using TMPro;
using System;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private TextMeshProUGUI entityIdText;
    private TextMeshProUGUI entityNameText;
    private TextMeshProUGUI entityPopulationTypeText;
    private TextMeshProUGUI entityGroupTypeText;

    private TextMeshProUGUI cellSolidText;
    private TextMeshProUGUI cellPositionText;
    private TextMeshProUGUI cellGroundTypeText;
    private TextMeshProUGUI cellBuildingTypeText;

    private InfoType mode;
    public InfoType Mode { get => mode; }


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

        entityIdText = UIUtil.SetLabel(
           "Id", entityTab.transform.Find("Id")
        );
        entityNameText = UIUtil.SetLabel(
            "Name", entityTab.transform.Find("Name")
        );
        entityPopulationTypeText = UIUtil.SetLabel(
            "Population", entityTab.transform.Find("Population Type")
        );
        entityGroupTypeText = UIUtil.SetLabel(
            "Group", entityTab.transform.Find("Group Type")
        );
    }


    private void SetupCellTab()
    {
        cellTab = GameObject.Find("Cell Tab");
        cellTab.SetActive(false);

        cellSolidText = UIUtil.SetLabel(
            "Solid", cellTab.transform.Find("Solid")
        );
        cellPositionText = UIUtil.SetLabel(
            "Position", cellTab.transform.Find("Position")
        );
        cellGroundTypeText = UIUtil.SetLabel(
            "Ground", cellTab.transform.Find("Ground Type")
        );
        cellBuildingTypeText = UIUtil.SetLabel(
            "Wall", cellTab.transform.Find("Wall Type")
        );
    }


    public void ActivateEntityMode(CitizenData citizenData)
    {
        mode = InfoType.Entity;

        entityTab.SetActive(true);
        cellTab.SetActive(false);

        entityIdText.text = $"{citizenData.IdData.IdNumber}";
        entityNameText.text = citizenData.IdData.FullName;
        entityPopulationTypeText.text = $"{Enum.GetName(typeof(PopulationType), citizenData.IdData.PopulationType)}";
        entityGroupTypeText.text = $"{Enum.GetName(typeof(GroupType), citizenData.GroupData.GroupType)}";
    }


    public void ActivateCellMode(CellData cellData)
    {
        mode = InfoType.Cell;

        entityTab.SetActive(false);
        cellTab.SetActive(true);

        cellSolidText.text = $"{cellData.Solid}";
        cellPositionText.text = $"{cellData.Position}";
        cellGroundTypeText.text = $"{Enum.GetName(typeof(GroundType), cellData.GroundType)}";
        cellBuildingTypeText.text = $"{Enum.GetName(typeof(WallType), cellData.WallType)}";
    }


    public void Deactivate()
    {
        mode = InfoType.None;

        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
