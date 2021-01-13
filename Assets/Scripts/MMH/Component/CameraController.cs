using UnityEngine;

namespace MMH
{
    public class CameraController : MonoBehaviour
    {
        private void Update()
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
