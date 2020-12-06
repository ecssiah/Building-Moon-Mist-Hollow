using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    private Text DataText;


    void Start()
    {
        DataText = GameObject.Find("Data").GetComponent<Text>();    
    }


    void Update()
    {
        
    }


    public void updateSelection(Vector3Int selection)
    {
        DataText.text = $"({selection.x},{selection.y})";
    }



}
