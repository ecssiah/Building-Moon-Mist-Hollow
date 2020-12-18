using TMPro;
using UnityEngine;

public struct UIUtil
{
    public static TextMeshProUGUI SetLabel(string label, Transform transform)
    {
        TextMeshProUGUI dataTextMesh = transform
            .Find("Data").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI labelTextMesh = transform
            .Find("Label").GetComponent<TextMeshProUGUI>();

        labelTextMesh.text = label;

        return dataTextMesh;
    }
}