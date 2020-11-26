using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapSelectionHandler : MonoBehaviour
{
    private SelectionHandler selectionHandler;


    void Start()
    {
        selectionHandler = GetComponentInParent<SelectionHandler>();
    }


    void Update()
    {
        
    }


    private void OnMouseDown()
    {
    }
}
