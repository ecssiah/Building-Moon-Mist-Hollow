using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MMH
{
    public class MainMenu : MonoBehaviour
    {
        private GameObject _mainMenuGameObject;

        private Type.MainMenuSection _mainMenuSection;

        private PopulationTab populationTab;
        private RulerTab rulerTab;
        private SettingsTab settingsTab;

        private AdminTab adminTab;

        public bool Active
        {
            get => _mainMenuGameObject.activeInHierarchy;
            set => _mainMenuGameObject.SetActive(value);
        }


        void Awake()
        {
            _mainMenuGameObject = GameObject.Find("Main Menu");
            _mainMenuGameObject.SetActive(false);

            _mainMenuSection = Type.MainMenuSection.None;

        }


        void Start()
        {
            populationTab = GameObject.Find("Population Tab").AddComponent<PopulationTab>();
            rulerTab = GameObject.Find("Ruler Tab").AddComponent<RulerTab>();
            settingsTab = GameObject.Find("Settings Tab").AddComponent<SettingsTab>();

            adminTab = GameObject.Find("Admin Tab").AddComponent<AdminTab>();

            Button populationButton = GameObject.Find("Population Button").GetComponent<Button>();
            populationButton.onClick.AddListener(delegate { SelectSection(Type.MainMenuSection.Population); });

            Button rulerButton = GameObject.Find("Ruler Button").GetComponent<Button>();
            rulerButton.onClick.AddListener(delegate { SelectSection(Type.MainMenuSection.Ruler); });

            Button settingsButton = GameObject.Find("Settings Button").GetComponent<Button>();
            settingsButton.onClick.AddListener(delegate { SelectSection(Type.MainMenuSection.Settings); });
        }


        private void SelectSection(Type.MainMenuSection mainMenuSection)
        {
            _mainMenuSection = mainMenuSection;

            populationTab.Active = false;
            rulerTab.Active = false;
            settingsTab.Active = false;
            adminTab.Active = false;

            switch (_mainMenuSection)
            {
                case Type.MainMenuSection.Population:
                    populationTab.Active = true;
                    break;
                case Type.MainMenuSection.Ruler:
                    rulerTab.Active = true;
                    break;
                case Type.MainMenuSection.Settings:
                    settingsTab.Active = true;
                    break;
                default:
                    break;
            }
        }
    }
}