using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Awake()
    {
        Camera.main.orthographicSize = ViewInfo.DefaultOrthographicSize;
    }


    void Update()
    {
        float dx = ViewInfo.CameraPanSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float dy = ViewInfo.CameraPanSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        Camera.main.transform.Translate(dx, dy, 0);

        float dz = ViewInfo.CameraZoomSpeed * Time.deltaTime * Input.GetAxis("Zoom");

        Camera.main.orthographicSize = Mathf.Clamp(
            Camera.main.orthographicSize + dz,
            ViewInfo.MinimumOrthographicSize,
            ViewInfo.MaximumOrthographicSize
        );
    }
}
