using UnityEngine;

public struct ViewUtil
{
    public static float GetCameraZoomRatio()
    {
        return ViewInfo.DefaultOrthographicSize / Camera.main.orthographicSize;
    }
}
