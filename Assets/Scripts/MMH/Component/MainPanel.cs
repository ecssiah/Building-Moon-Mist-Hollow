using UnityEngine;
using UnityEngine.EventSystems;


namespace MMH
{
    public class MainPanel : MonoBehaviour
    {
        private GameObject panelObject;

        private GameObject adminTab;
        private GameObject settingsTab;

        void Awake()
        {
            panelObject = GameObject.Find("Main Panel");

            adminTab = GameObject.Find("Admin Tab");
            settingsTab = GameObject.Find("Settings Tab");

            panelObject.SetActive(false);

            adminTab.SetActive(false);
            settingsTab.SetActive(false);
        }


        public void Toggle()
        {
            panelObject.SetActive(!panelObject.activeInHierarchy);
        }
    }
}