using Unity.Mathematics;
using UnityEngine;


namespace MMH.System
{
    public class UISystem : MonoBehaviour, Handler.IUserInterfaceEventHandler
    {
        private RenderSystem renderSystem;
        private MapSystem mapSystem;

        private Data.UI uiData;

        private Component.MainMenu mainMenu;
        private Component.InfoWindow infoWindow;
        private Component.EntityLabeler entityLabeler;


        void Awake()
        {
            uiData = new Data.UI
            {
                Mode = Type.UIMode.None,
            };

            renderSystem = GameObject.Find("Render System").GetComponent<RenderSystem>();
            mapSystem = GameObject.Find("Map System").GetComponent<MapSystem>();

            entityLabeler = gameObject.AddComponent<Component.EntityLabeler>();

            mainMenu = transform.Find("Canvas/Main Panel/Main Menu").gameObject.AddComponent<Component.MainMenu>();
            infoWindow = transform.Find("Canvas/Info Panel/Info Window").gameObject.AddComponent<Component.InfoWindow>();

            _ = gameObject.AddComponent<Component.CameraController>();
            _ = gameObject.AddComponent<Component.SelectionHandler>();
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleMain();
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                SetMode(Type.UIMode.Main);

                mainMenu.SelectSection(Type.MainMenuSection.Admin);
            }
        }


        private void ToggleMain()
        {
            if (uiData.Mode == Type.UIMode.None)
            {
                SetMode(Type.UIMode.Main);
            }
            else
            {
                SetMode(Type.UIMode.None);
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

            entityLabeler.SelectEntity(entity);

            Component.Colonist colonist = entity.GetComponent<Component.Colonist>();
            infoWindow.DisplayColonist(colonist);
        }


        public void SelectCell(int2 cellPosition)
        {
            if (uiData.Mode == Type.UIMode.Main) return;

            ClearSelection();

            mapSystem.Map.SelectCell(cellPosition);
            renderSystem.SetOverlay(cellPosition, Type.Overlay.Selection);

            Data.Cell cellData = mapSystem.Map.GetCell(cellPosition);
            infoWindow.ActivateCellMode(cellData);
        }


        public void ClearSelection()
        {
            renderSystem.SetOverlay(mapSystem.Map.SelectedCell, Type.Overlay.None);

            entityLabeler.Clear();
            infoWindow.Deactivate();
        }
    }
}