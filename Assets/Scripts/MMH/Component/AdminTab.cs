using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminTab : Tab
{
    private GameObject testButtonObject;

    private Button testButton;

    private TextMeshProUGUI testTextContent;

    public override void Awake()
    {
        Active = false;

        testTextContent = gameObject.AddComponent<TextMeshProUGUI>();
        testTextContent.text = "Admin";

        testButtonObject = transform.Find("Test Button").gameObject;
        testButton = testButtonObject.GetComponent<Button>();

        testButton.onClick.AddListener(delegate { OnTestButtonClick(); });
    }


    private void OnTestButtonClick()
    {
        print("Test Button Clicked!");
    }
}
