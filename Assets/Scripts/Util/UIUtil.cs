using TMPro;
using UnityEngine;

public struct UIUtil
{
    public static TextMeshProUGUI SetLabel(string label, Transform transform)
    {
        TextMeshProUGUI textMesh = transform
            .Find("Data").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI labelMesh = transform
            .Find("Label").GetComponent<TextMeshProUGUI>();

        labelMesh.text = label;

        return transform.Find("Data").GetComponent<TextMeshProUGUI>();
    }


    public static float CameraZoomRatio()
    {
        return ViewInfo.DefaultOrthographicSize / Camera.main.orthographicSize;
    }
}