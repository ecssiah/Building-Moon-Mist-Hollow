using UnityEngine;
using TMPro;
using System;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private TextMeshProUGUI entityNameText;
    private TextMeshProUGUI entityCitizenNumberText;

    private TextMeshProUGUI cellPositionText;
    private TextMeshProUGUI cellGroundTypeText;
    private TextMeshProUGUI cellBuildingTypeText;

    private InfoType mode;
    public InfoType Mode { get => mode; }

    private CitizenData entityData;
    public CitizenData EntityData { get => entityData; }

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

        entityCitizenNumberText = UIUtil.SetLabel(
           "Citizen", entityTab.transform.Find("Citizen Number")
        );
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
        cellGroundTypeText = UIUtil.SetLabel(
            "Ground", cellTab.transform.Find("Ground Type")
        );
        cellBuildingTypeText = UIUtil.SetLabel(
            "Wall", cellTab.transform.Find("Wall Type")
        );
    }


    public void ActivateEntityMode(CitizenData entityData)
    {
        mode = InfoType.Entity;

        entityTab.SetActive(true);
        cellTab.SetActive(false);

        entityNameText.text = entityData.name;
        entityCitizenNumberText.text = $"{entityData.citizenNumber}";
    }


    public void ActivateCellMode(CellData cellData)
    {
        mode = InfoType.Cell;

        entityTab.SetActive(false);
        cellTab.SetActive(true);

        cellPositionText.text = $"{cellData.position}";
        cellGroundTypeText.text = $"{Enum.GetName(typeof(GroundType), cellData.groundType)}";
        cellBuildingTypeText.text = $"{Enum.GetName(typeof(WallType), cellData.wallType)}";
    }


    public void Deactivate()
    {
        mode = InfoType.None;

        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
