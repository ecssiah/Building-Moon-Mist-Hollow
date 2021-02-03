using Unity.Mathematics;
using UnityEngine;


namespace MMH.System
{
    public class UISystem : MonoBehaviour, Handler.IUIEventHandler
    {
        private Data.UI uiData;

        private RenderSystem renderSystem;
        private MapSystem mapSystem;

        private MainMenu mainMenu;
        private InfoWindow infoWindow;

        private EntityLabeler entityLabeler;


        void Awake()
        {
            uiData = new Data.UI
            {
                Mode = Type.UIMode.None,
            };

            renderSystem = GameObject.Find("RenderSystem").GetComponent<RenderSystem>();
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            mainMenu = GameObject.Find("Main Menu").AddComponent<MainMenu>();
            infoWindow = GameObject.Find("Info Window").AddComponent<InfoWindow>();

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
                else
                {
                    SetMode(Type.UIMode.None);
                }
            }

            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                SetMode(Type.UIMode.Admin);
            }
        }


        public void SetMode(Type.UIMode uiMode)
        {
            uiData.Mode = uiMode;

            switch (uiMode)
            {
                case Type.UIMode.None:
                    if (mainMenu.Section == Type.MainMenuSection.Admin)
                    {
                        mainMenu.SelectSection(Type.MainMenuSection.Ruler);
                    }

                    mainMenu.Active = false;
                    break;
                case Type.UIMode.Main:
                    mainMenu.Active = true;
                    break;
                case Type.UIMode.Admin:
                    mainMenu.Active = true;
                    mainMenu.SelectSection(Type.MainMenuSection.Admin);
                    break;
            }
        }


        public void SelectEntity(GameObject entity)
        {
            if (uiData.Mode == Type.UIMode.Main) return;

            ClearSelection();

            entityLabeler.SelectEntity(entity);

            Colonist colonist = entity.GetComponent<Colonist>();
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