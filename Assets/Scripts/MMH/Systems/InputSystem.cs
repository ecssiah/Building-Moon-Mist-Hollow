using UnityEngine;

namespace MMH
{
    namespace System
    {
        public class InputSystem : MonoBehaviour
        {
            private UISystem uiSystem;
            private MapSystem mapSystem;

            private SelectionHandler selectionHandler;


            void Awake()
            {
                uiSystem = GameObject.Find("UISystem").GetComponent<UISystem>();
                mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

                selectionHandler = gameObject.AddComponent<SelectionHandler>();
                selectionHandler.BroadcastEntitySelection = OnEntitySelection;
                selectionHandler.BroadcastCellSelection = OnCellSelection;
            }


            void Update()
            {
                if (UnityEngine.Input.GetKeyDown(KeyCode.Mouse0))
                {
                    selectionHandler.Select();
                }

                if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                {
                    uiSystem.ClearSelection();
                    mapSystem.ClearSelection();
                }
            }


            public void OnEntitySelection(GameObject entity)
            {
                uiSystem.ClearSelection();
                mapSystem.ClearSelection();

                uiSystem.SelectEntity(entity);
            }


            public void OnCellSelection(Vector2Int cellPosition)
            {
                uiSystem.ClearSelection();
                mapSystem.ClearSelection();

                uiSystem.SelectCell(cellPosition);
                mapSystem.SelectCell(cellPosition);
            }
        }
    }
}