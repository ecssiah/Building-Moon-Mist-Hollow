using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public GameObject entityLabelPrefab;

    private GameObject labelsObject;

    void Start()
    {
        Init();
    }


    void Update()
    {
        
    }


    private void Init()
    {
        labelsObject = GameObject.Find("Labels");

        Vector3 worldPosition = new Vector3(0, 0, 0);
        Vector3 screenPosition = Utilities.WorldToScreen(worldPosition);

        GameObject newLabel = Instantiate(
            entityLabelPrefab, screenPosition, Quaternion.identity
        );

        TextMeshProUGUI text = newLabel.GetComponent<TextMeshProUGUI>();
        text.text = "Entity Name";

        newLabel.transform.SetParent(labelsObject.transform, true);
    }
}
