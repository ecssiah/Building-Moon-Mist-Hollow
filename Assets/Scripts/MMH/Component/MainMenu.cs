using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MMH.Component
{
    public class MainMenu : MonoBehaviour
    {
        public Type.MainMenuSection Section { get; private set; }

        private Button populationButton;
        private Button rulerButton;
        private Button settingsButton;

        private PopulationTab populationTab;
        private RulerTab rulerTab;
        private SettingsTab settingsTab;
        private AdminTab adminTab;

        public bool Active
        {
            get => gameObject.activeInHierarchy;
            set
            {
                gameObject.SetActive(value);
            }
        }

        void Awake()
        {
            Active = false;
        }


        void Start()
        {
            populationTab = GameObject.Find("Population Tab").AddComponent<Component.PopulationTab>();
            rulerTab = GameObject.Find("Ruler Tab").AddComponent<Component.RulerTab>();
            settingsTab = GameObject.Find("Settings Tab").AddComponent<Component.SettingsTab>();
            adminTab = GameObject.Find("Admin Tab").AddComponent<Component.AdminTab>();

            populationButton = GameObject.Find("Population Button").GetComponent<Button>();
            populationButton.onClick.AddListener(
                delegate { SelectSection(Type.MainMenuSection.Population); }
            );

            rulerButton = GameObject.Find("Ruler Button").GetComponent<Button>();
            rulerButton.onClick.AddListener(
                delegate { SelectSection(Type.MainMenuSection.Ruler); }
            );

            settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
            settingsButton.onClick.AddListener(
                delegate { SelectSection(Type.MainMenuSection.Settings); }
            );

            SelectSection(Type.MainMenuSection.Ruler);
        }


        public void SelectSection(Type.MainMenuSection mainMenuSection)
        {
            Section = mainMenuSection;

            populationTab.Active = false;
            rulerTab.Active = false;
            settingsTab.Active = false;
            adminTab.Active = false;

            switch (Section)
            {
                case Type.MainMenuSection.Population:
                    populationTab.Active = true;
                    populationButton.Select();
                    break;
                case Type.MainMenuSection.Ruler:
                    rulerTab.Active = true;
                    rulerButton.Select();
                    break;
                case Type.MainMenuSection.Settings:
                    settingsTab.Active = true;
                    settingsButton.Select();
                    break;
                case Type.MainMenuSection.Admin:
                    adminTab.Active = true;
                    EventSystem.current.SetSelectedGameObject(null);
                    break;
                case Type.MainMenuSection.None:
                default:
                    break;
            }
        }
    }
}