using UnityEngine;
using TMPro;
using System;

namespace MMH
{
    public class InfoWindow : MonoBehaviour
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

        private Type.Info mode;
        public Type.Info Mode { get => mode; }


        void Awake()
        {
            SetupEntityTab();
            SetupCellTab();

            mode = Type.Info.None;
        }


        private void SetupEntityTab()
        {
            entityTab = GameObject.Find("Entity Tab");
            entityTab.SetActive(false);

            entityIdText = Util.UI.SetLabel("Id", entityTab.transform.Find("Id"));
            entityNameText = Util.UI.SetLabel("Name", entityTab.transform.Find("Name"));
            entityPopulationTypeText = Util.UI.SetLabel("Population", entityTab.transform.Find("Population Type"));
            entityGroupTypeText = Util.UI.SetLabel("Group", entityTab.transform.Find("Group Type"));
        }


        private void SetupCellTab()
        {
            cellTab = GameObject.Find("Cell Tab");
            cellTab.SetActive(false);

            cellSolidText = Util.UI.SetLabel("Solid", cellTab.transform.Find("Solid"));
            cellPositionText = Util.UI.SetLabel("Position", cellTab.transform.Find("Position"));
            cellGroundTypeText = Util.UI.SetLabel("Ground", cellTab.transform.Find("Ground Type"));
            cellBuildingTypeText = Util.UI.SetLabel("Wall", cellTab.transform.Find("Wall Type"));
        }


        public void DisplayCitizen(Citizen citizen)
        {
            mode = Type.Info.Citizen;

            entityTab.SetActive(true);
            cellTab.SetActive(false);

            entityIdText.text = $"{citizen.Id.Number}";
            entityNameText.text = citizen.Id.FullName;
            entityPopulationTypeText.text = $"{Enum.GetName(typeof(Type.Population), citizen.Id.PopulationType)}";
            entityGroupTypeText.text = $"{Enum.GetName(typeof(Type.Group), citizen.Id.GroupType)}";
        }


        public void ActivateCellMode(Data.Cell cell)
        {
            mode = Type.Info.Cell;

            entityTab.SetActive(false);
            cellTab.SetActive(true);

            cellSolidText.text = $"{cell.Solid}";
            cellPositionText.text = $"{cell.Position}";
            cellGroundTypeText.text = $"{Enum.GetName(typeof(Type.Ground), cell.GroundType)}";
            cellBuildingTypeText.text = $"{Enum.GetName(typeof(Type.Wall), cell.WallType)}";
        }


        public void Deactivate()
        {
            mode = Type.Info.None;

            entityTab.SetActive(false);
            cellTab.SetActive(false);
        }
    }
}
