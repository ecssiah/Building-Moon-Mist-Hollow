using System.Collections;
using System.Collections.Generic;
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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bool success = ExecuteEvents.Execute<UIMessageTarget>(gameObject, null, (x, y) => x.ToggleMainPanel());

                Debug.Log(success);
            }
        }


        public void ToggleActive()
        {
            panelObject.SetActive(!panelObject.activeInHierarchy);
        }
    }
}