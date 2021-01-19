using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminTab : Tab
{
    private TextMeshProUGUI testTextContent;

    public override void Awake()
    {
        Active = false;

        testTextContent = gameObject.AddComponent<TextMeshProUGUI>();
        testTextContent.text = "Admin";
    }
}
