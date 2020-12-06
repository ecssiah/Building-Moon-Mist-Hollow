using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    private TextMeshProUGUI DataText;


    void Start()
    {
        DataText = GameObject.Find("Data").GetComponent<TextMeshProUGUI>();    
    }


    void Update()
    {
        
    }


    public void updateSelection(Vector3Int selection)
    {
        DataText.text = $"({selection.x},{selection.y})";
    }



}
