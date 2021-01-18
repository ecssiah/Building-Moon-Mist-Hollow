using UnityEngine;


namespace MMH.System
{
    public class UISystem : MonoBehaviour, Handler.IUIEventHandler
    {
        private Data.UI uiData;

        private MapSystem mapSystem;

        private MainMenu mainMenu;
        private InfoWindow infoWindow;

        private EntityLabeler entityLabeler;


        void Awake()
        {
            uiData = new Data.UI
            {
                Mode = Type.UIMode.None
            };

            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            mainMenu = GameObject.Find("Main Menu").AddComponent<MainMenu>();
            infoWindow = gameObject.AddComponent<InfoWindow>();

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
                    mainMenu.Active = false;
                    break;
                case Type.UIMode.Main:
                    mainMenu.Active = true;
                    break;
            }
        }


        public void SelectEntity(GameObject entity)
        {
            if (uiData.Mode == Type.UIMode.Main) return;

            ClearSelection();
            mapSystem.ClearSelection();

            entityLabeler.SelectEntity(entity);

            Citizen citizen = entity.GetComponent<Citizen>();
            infoWindow.DisplayCitizen(citizen);
        }


        public void SelectCell(Vector2Int cellPosition)
        {
            if (uiData.Mode == Type.UIMode.Main) return;

            ClearSelection();
            mapSystem.ClearSelection();

            mapSystem.SelectCell(cellPosition);

            Data.Cell cellData = mapSystem.GetCell(cellPosition);
            infoWindow.ActivateCellMode(cellData);
         }


        public void ClearSelection()
        {
            mapSystem.ClearSelection();

            entityLabeler.Clear();
            infoWindow.Deactivate();
        }
    }
}