using Unity.Mathematics;
using UnityEngine;


namespace MMH.System
{
    public class UISystem : MonoBehaviour, Handler.IUIEventHandler
    {
        private Data.UI uiData;

        private RenderSystem renderSystem;
        private MapSystem mapSystem;

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

            mainMenu = GameObject.Find("Main Menu").AddComponent<Component.MainMenu>();
            infoWindow = GameObject.Find("Info Window").AddComponent<Component.InfoWindow>();

            entityLabeler = gameObject.AddComponent<Component.EntityLabeler>();

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
            Type.UIMode uiMode = uiData.Mode == Type.UIMode.None ? Type.UIMode.Main : Type.UIMode.None;

            SetMode(uiMode);
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

            renderSystem.SetOverlay(cellPosition, Type.Overlay.None);

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