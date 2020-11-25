using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    private SelectionData selectionData;


    void Start()
    {
        selectionData = GetComponentInParent<SelectionData>();    
    }


    void Update()
    {

    }


    private void OnMouseDown()
    {
        Vector3 selectedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 isoGridVector = Utilities.ToIsoGrid(selectedPosition);

        selectionData.selectedCell = isoGridVector;
    }
}
