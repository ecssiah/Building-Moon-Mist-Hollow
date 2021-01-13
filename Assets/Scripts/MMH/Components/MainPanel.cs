using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMH
{
    public class MainPanel : MonoBehaviour, Handler.IUIMessages
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
                bool success = ExecuteEvents.Execute<Handler.IUIMessages>(
                    gameObject,
                    null,
                    (x, y) => x.ToggleMainPanel()
                );
            }
        }


        public void ToggleMainPanel()
        {
            panelObject.SetActive(!panelObject.activeInHierarchy);
        }
    }
}