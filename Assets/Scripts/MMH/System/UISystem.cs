using UnityEngine;
using UnityEngine.EventSystems;


namespace MMH.System
{
    public class UISystem : MonoBehaviour, Handler.ISelectionHandler
    {
        private MapSystem mapSystem;

        private InfoPanel infoPanel;

        private SelectionHandler selectionHandler;
        private EntityLabeler entityLabeler;


        void Awake()
        {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            infoPanel = gameObject.AddComponent<InfoPanel>();

            _ = gameObject.AddComponent<MainPanel>();
            _ = gameObject.AddComponent<SelectionHandler>();

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