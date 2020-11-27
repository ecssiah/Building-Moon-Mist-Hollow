using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    private Text cellLabel;


    void Start()
    {
        cellLabel = GameObject.Find("Cell Info").GetComponent<Text>();    
    }


    void Update()
    {
        
    }


    public void updateSelection(Vector3Int selection)
    {
        cellLabel.text = $"({selection.x},{selection.y})";
    }



}
