﻿using System;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public Action<Vector3Int> BroadcastCellSelection;


    public void Select()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {

        }
        else
        {
            BroadcastCellSelection(Utilities.GetWorldPoint(Input.mousePosition));
        }
    }

}