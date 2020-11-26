using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionHandler : MonoBehaviour
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
        selectionHandler.OnCharacterSelection();
    }
}
