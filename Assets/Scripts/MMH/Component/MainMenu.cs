using UnityEngine;
using UnityEngine.EventSystems;


namespace MMH
{
    public class MainMenu : MonoBehaviour
    {
        private GameObject mainMenuGameObject;

        private PopulationTab populationTab;
        private RulerTab rulerTab;
        private SettingsTab settingsTab;

        private AdminTab adminTab;

        public bool Active
        {
            get => mainMenuGameObject.activeInHierarchy;
            set => mainMenuGameObject.SetActive(value);
        }


        void Awake()
        {
            mainMenuGameObject = GameObject.Find("Main Menu");
            mainMenuGameObject.SetActive(false);

        }


        void Start()
        {
        }
    }
}