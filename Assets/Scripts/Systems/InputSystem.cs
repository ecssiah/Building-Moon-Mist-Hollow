using UnityEngine;
using System.Collections;

public class InputSystem : MonoBehaviour
{
    private SelectionHandler selectionHandler;


    void Awake()
    {
    }


    void Start()
    {
        selectionHandler = gameObject.AddComponent<SelectionHandler>();

    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionHandler.Select();
        }
    }
}
