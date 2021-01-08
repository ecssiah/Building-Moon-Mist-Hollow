using TMPro;
using UnityEngine;


namespace MMH
{
    namespace Util
    {
        public struct UI
        {
            public static TextMeshProUGUI SetLabel(string label, Transform transform)
            {
                TextMeshProUGUI textMesh = transform.Find("Data").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI labelMesh = transform.Find("Label").GetComponent<TextMeshProUGUI>();

                labelMesh.text = label;

                return textMesh;
            }
        }
    }
}
