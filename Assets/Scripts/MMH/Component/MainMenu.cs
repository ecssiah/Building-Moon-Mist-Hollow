﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MMH
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
            Section = Type.MainMenuSection.None;

            populationTab = GameObject.Find("Population Tab").AddComponent<PopulationTab>();
            rulerTab = GameObject.Find("Ruler Tab").AddComponent<RulerTab>();
            settingsTab = GameObject.Find("Settings Tab").AddComponent<SettingsTab>();
            adminTab = GameObject.Find("Admin Tab").AddComponent<AdminTab>();
        }


        void Start()
        {
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
                    populationButton.Select();
                    populationTab.Active = true;
                    break;
                case Type.MainMenuSection.Ruler:
                    rulerButton.Select();
                    rulerTab.Active = true;
                    break;
                case Type.MainMenuSection.Settings:
                    settingsButton.Select();
                    settingsTab.Active = true;
                    break;
                case Type.MainMenuSection.Admin:
                    EventSystem.current.SetSelectedGameObject(null);
                    adminTab.Active = true;
                    break;
                case Type.MainMenuSection.None:
                default:
                    break;
            }
        }
    }
}