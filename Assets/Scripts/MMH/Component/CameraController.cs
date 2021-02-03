using UnityEngine;

namespace MMH.Component
{
    public class CameraController : MonoBehaviour
    {
        private void Update()
        {
            float dx = Info.Render.CameraPanSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
            float dy = Info.Render.CameraPanSpeed * Time.deltaTime * Input.GetAxis("Vertical");
            float dz = Info.Render.CameraZoomSpeed * Time.deltaTime * Input.GetAxis("Zoom");

            Camera.main.transform.Translate(dx, dy, 0);

            Camera.main.orthographicSize = Mathf.Clamp(
                Camera.main.orthographicSize + dz,
                Info.Render.MinimumOrthographicSize,
                Info.Render.MaximumOrthographicSize
            );
        }
    }
}
