using UnityEngine;
using UnityEngine.EventSystems;


namespace MMH
{
    public class MainPanel : MonoBehaviour
    {
        private GameObject panelObject;

        private GameObject rulerTab;
        private GameObject populationTab;
        private GameObject settingsTab;
        private GameObject adminTab;

        public bool Active
        {
            get => panelObject.activeInHierarchy;
            set => panelObject.SetActive(value);
        }

        void Awake()
        {
            panelObject = GameObject.Find("Main Panel");

            rulerTab = GameObject.Find("Ruler Tab");
            populationTab = GameObject.Find("Population Tab");
            settingsTab = GameObject.Find("Settings Tab");
            adminTab = GameObject.Find("Admin Tab");

            panelObject.SetActive(false);

            rulerTab.SetActive(false);
            populationTab.SetActive(false);
            adminTab.SetActive(false);
            settingsTab.SetActive(false);
        }
    }
}