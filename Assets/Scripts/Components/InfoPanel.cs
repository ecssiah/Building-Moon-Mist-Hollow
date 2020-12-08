using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;


    void Awake()
    {
        entityTab = GameObject.Find("Entity Tab");
        cellTab = GameObject.Find("Cell Tab");

        entityTab.SetActive(false);
        cellTab.SetActive(false);
    }


    void Start()
    {
    }


    void Update()
    {
        
    }

}
