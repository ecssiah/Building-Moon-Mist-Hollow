﻿using UnityEngine;
using UnityEngine.EventSystems;


namespace MMH.System
{
    public class UISystem : MonoBehaviour
    {
        private MapSystem mapSystem;

        private InfoPanel infoPanel;

        private EntityLabeler entityLabeler;


        void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            infoPanel = gameObject.AddComponent<InfoPanel>();
            _ = gameObject.AddComponent<MainPanel>();

            entityLabeler = gameObject.AddComponent<EntityLabeler>();
        }


        public void SelectEntity(GameObject entity)
        {
            Citizen citizen = entity.GetComponent<Citizen>();

            entityLabeler.SelectEntity(entity);

            infoPanel.DisplayCitizen(citizen);
        }


        public void SelectCell(Vector2Int cellPosition)
        {
            Data.Cell cellData = mapSystem.GetCell(cellPosition);

            infoPanel.ActivateCellMode(cellData);
        }


        public void ClearSelection()
        {
            entityLabeler.Clear();

            infoPanel.Deactivate();
        }
    }
}