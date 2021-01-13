using UnityEngine;
using UnityEngine.EventSystems;

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

            gameObject.AddComponent<SelectionHandler>();
        }


        void Update()
        {
            UpdateCamera();
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


        private void UpdateCamera()
        {
            float dx = Info.View.CameraPanSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            float dy = Info.View.CameraPanSpeed * Time.deltaTime * Input.GetAxis("Vertical");
            float dz = Info.View.CameraZoomSpeed * Time.deltaTime * Input.GetAxis("Zoom");

            Camera.main.transform.Translate(dx, dy, 0);

            Camera.main.orthographicSize = Mathf.Clamp(
                Camera.main.orthographicSize + dz,
                Info.View.MinimumOrthographicSize,
                Info.View.MaximumOrthographicSize
            );
        }
    }
}