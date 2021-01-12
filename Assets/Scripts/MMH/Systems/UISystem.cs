﻿using UnityEngine;

namespace MMH.System
{
    public class UISystem : MonoBehaviour
    {
        private MapSystem mapSystem;

        private EntityLabeler entityLabeler;
        private InfoPanel infoPanel;


        void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            entityLabeler = gameObject.AddComponent<EntityLabeler>();
            infoPanel = gameObject.AddComponent<InfoPanel>();
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