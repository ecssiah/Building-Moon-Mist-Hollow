using UnityEngine;
using TMPro;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private TextMeshProUGUI entityNameText;
    private TextMeshProUGUI cellPositionText;

    private InfoType mode;
    public InfoType Mode { get => mode; }

    private EntityData entityData;
    public EntityData EntityData { get => entityData; }

    private CellData cellData;
    public CellData CellData { get => cellData; }


    void Awake()
    {
        entityTab = GameObject.Find("Entity Tab");
        cellTab = GameObject.Find("Cell Tab");

        Transform entityNameTransform = entityTab.transform.Find("Name");
        Transform cellPositionTransform = cellTab.transform.Find("Position");

        entityNameText = entityNameTransform
            .Find("Data").GetComponent<TextMeshProUGUI>();
        cellPositionText = cellPositionTransform
            .Find("Data").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI entityNameLabel = entityNameTransform
            .Find("Label").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cellPositionLabel = cellPositionTransform
            .Find("Label").GetComponent<TextMeshProUGUI>();

        entityNameLabel.text = "Name: ";
        cellPositionLabel.text = "Position: ";

        Deactivate();
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
    }


    public void Deactivate()
    {
        mode = InfoType.None;

        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }
}
