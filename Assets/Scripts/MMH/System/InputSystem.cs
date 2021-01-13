using UnityEngine;

namespace MMH.System
{
    public class InputSystem : MonoBehaviour, Handler.ISelectionHandler
    {
        private UISystem uiSystem;
        private MapSystem mapSystem;

        void Awake()
        {
            uiSystem = GameObject.Find("UISystem").GetComponent<UISystem>();
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            gameObject.AddComponent<CameraController>();
            gameObject.AddComponent<SelectionHandler>();
        }


        public void SelectEntity(GameObject entity)
        {
            mapSystem.ClearSelection();

            uiSystem.ClearSelection();
            uiSystem.SelectEntity(entity);
        }


        public void SelectCell(Vector2Int cellPosition)
        {
            mapSystem.ClearSelection();
            mapSystem.SelectCell(cellPosition);

            uiSystem.ClearSelection();
            uiSystem.SelectCell(cellPosition);
        }


        public void ClearSelection()
        {
            uiSystem.ClearSelection();
            mapSystem.ClearSelection();
        }
    }
}