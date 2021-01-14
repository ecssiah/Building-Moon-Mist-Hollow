using UnityEngine;


namespace MMH.System
{
    public class UISystem : MonoBehaviour, Handler.IUIEventHandler
    {
        private MapSystem mapSystem;

        private MainPanel mainPanel;
        private InfoPanel infoPanel;

        private EntityLabeler entityLabeler;


        void Awake()
        {
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
                mainPanel.Toggle();
            }
        }


        public void SelectEntity(GameObject entity)
        {
            ClearSelection();
            mapSystem.ClearSelection();

            Citizen citizen = entity.GetComponent<Citizen>();
            entityLabeler.SelectEntity(entity);
            infoPanel.DisplayCitizen(citizen);
        }


        public void SelectCell(Vector2Int cellPosition)
        {
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