using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    private GameObject entityTab;
    private GameObject cellTab;

    private InfoMode mode;

    public InfoMode Mode
    {
        get => mode;

        set
        {
            mode = value;

            switch (mode)
            {
                case InfoMode.None:
                    entityTab.SetActive(false);
                    cellTab.SetActive(false);
                    break;
                case InfoMode.Entity:
                    entityTab.SetActive(true);
                    cellTab.SetActive(false);
                    break;
                case InfoMode.Cell:
                    entityTab.SetActive(false);
                    cellTab.SetActive(true);
                    break;
                default:
                    entityTab.SetActive(false);
                    cellTab.SetActive(false);
                    break;
            }
        }
    }


    void Awake()
    {
        entityTab = GameObject.Find("Entity Tab");
        cellTab = GameObject.Find("Cell Tab");
    }


    void Start()
    {
        Mode = InfoMode.None;
    }


    void Update()
    {
        
    }
}
