using UnityEngine;

public struct ViewUtil
{
    public static float CameraZoomRatio
    {
        get
        {
            return ViewInfo.DefaultOrthographicSize / Camera.main.orthographicSize;
        }
    }
}
