using UnityEngine;

namespace MMH
{
    namespace System
    {
        public class Input : MonoBehaviour
        {
            private UI uiSystem;
            private Map mapSystem;

            private SelectionHandler selectionHandler;


            void Awake()
            {
                uiSystem = GameObject.Find("UI").GetComponent<UI>();
                mapSystem = GameObject.Find("Map").GetComponent<Map>();

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