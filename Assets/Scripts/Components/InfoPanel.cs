using System;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private TextMeshProUGUI entityNameTextMesh;

    private TextMeshProUGUI cellPositionTextMesh;
    private TextMeshProUGUI cellCellTypeTextMesh;
    private TextMeshProUGUI cellBuildingTypeTextMesh;

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

        entityNameTextMesh = UIUtil.SetLabel(
            "Name", entityTab.transform.Find("Name")
        );

    }


    private void SetupCellTab()
    {
        cellTab = GameObject.Find("Cell Tab");
        cellTab.SetActive(false);

        cellPositionTextMesh = UIUtil.SetLabel(
            "Position", cellTab.transform.Find("Position")
        );

        cellCellTypeTextMesh = UIUtil.SetLabel(
            "Cell", cellTab.transform.Find("Cell Type")
        );

        cellBuildingTypeTextMesh = UIUtil.SetLabel(
            "Building", cellTab.transform.Find("Building Type")
        );
    }


    public void ActivateEntityMode(EntityData entityData)
    {
        mode = InfoType.Entity;

        entityTab.SetActive(true);
        cellTab.SetActive(false);

        entityNameTextMesh.text = entityData.name;
    }


    public void ActivateCellMode(CellData cellData)
    {
        mode = InfoType.Cell;

        entityTab.SetActive(false);
        cellTab.SetActive(true);

        cellPositionTextMesh.text = $"{cellData.position}";
        cellCellTypeTextMesh.text = $"{Enum.GetName(typeof(CellType), cellData.cellType)}";
        cellBuildingTypeTextMesh.text = $"{Enum.GetName(typeof(BuildingType), cellData.buildingType)}";
    }


    public void Deactivate()
    {
        mode = InfoType.None;

        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
