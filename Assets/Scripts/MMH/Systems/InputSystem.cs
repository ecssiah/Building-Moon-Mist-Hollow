using UnityEngine;

namespace MMH.System
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                selectionHandler.Select();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                uiSystem.ClearSelection();
                mapSystem.ClearSelection();
            }

            UpdateCamera();
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