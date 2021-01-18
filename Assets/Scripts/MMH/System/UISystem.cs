﻿using UnityEngine;


namespace MMH.System
{
    public class UISystem : MonoBehaviour, Handler.IUIEventHandler
    {
        private Data.UI uiData;

        private MapSystem mapSystem;

        private MainPanel mainPanel;
        private InfoPanel infoPanel;

        private EntityLabeler entityLabeler;


        void Awake()
        {
            uiData = new Data.UI
            {
                Mode = Type.UIMode.None
            };

            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            mainPanel = gameObject.AddComponent<MainPanel>();
            infoPanel = gameObject.AddComponent<InfoPanel>();

            entityLabeler = gameObject.AddComponent<EntityLabeler>();

            _ = gameObject.AddComponent<CameraController>();
            _ = gameObject.AddComponent<SelectionHandler>();
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (uiData.Mode == Type.UIMode.None)
                {
                    SetMode(Type.UIMode.Main);
                }
                else if (uiData.Mode == Type.UIMode.Main)
                {
                    SetMode(Type.UIMode.None);
                }
            }
        }


        public void SetMode(Type.UIMode uiMode)
        {
            uiData.Mode = uiMode;

            switch (uiMode)
            {
                case Type.UIMode.None:
                    mainPanel.Active = false;
                    break;
                case Type.UIMode.Main:
                    mainPanel.Active = true;
                    break;
            }
        }


        public void SelectEntity(GameObject entity)
        {
            if (uiData.Mode == Type.UIMode.Main) return;

            ClearSelection();
            mapSystem.ClearSelection();

            Citizen citizen = entity.GetComponent<Citizen>();
            entityLabeler.SelectEntity(entity);
            infoPanel.DisplayCitizen(citizen);
        }


        public void SelectCell(Vector2Int cellPosition)
        {
            if (uiData.Mode == Type.UIMode.Main) return;

            ClearSelection();
            mapSystem.ClearSelection();

            mapSystem.SelectCell(cellPosition);

            Data.Cell cellData = mapSystem.GetCell(cellPosition);
            infoPanel.ActivateCellMode(cellData);
         }


        public void ClearSelection()
        {
            mapSystem.ClearSelection();

            entityLabeler.Clear();
            infoPanel.Deactivate();
        }
    }
}