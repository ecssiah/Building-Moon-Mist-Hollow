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

        }


        void Start()
        {
            populationTab = transform.Find("Body Panel/Population Tab").gameObject.AddComponent<PopulationTab>();
            rulerTab = transform.Find("Body Panel/Ruler Tab").gameObject.AddComponent<RulerTab>();
            settingsTab = transform.Find("Body Panel/Settings Tab").gameObject.AddComponent<SettingsTab>();
            adminTab = transform.Find("Body Panel/Admin Tab").gameObject.AddComponent<AdminTab>();

            populationButton = transform.Find("Header Panel/Population Button").GetComponent<Button>();
            populationButton.onClick.AddListener(
                delegate { SelectSection(Type.MainMenuSection.Population); }
            );

            rulerButton = transform.Find("Header Panel/Ruler Button").GetComponent<Button>();
            rulerButton.onClick.AddListener(
                delegate { SelectSection(Type.MainMenuSection.Ruler); }
            );

            settingsButton = transform.Find("Header Panel/Settings Button").GetComponent<Button>();
            settingsButton.onClick.AddListener(
                delegate { SelectSection(Type.MainMenuSection.Settings); }
            );

            SelectSection(Type.MainMenuSection.Ruler);

            Active = false;
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