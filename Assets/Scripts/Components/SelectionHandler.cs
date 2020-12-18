﻿using System;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public Action<Vector2Int> BroadcastCellSelection;
    public Action<GameObject> BroadcastEntitySelection;


    public void Select()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            bool isTarget = hit.collider.gameObject.CompareTag("Target");

            if (isTarget)
            {
                BroadcastEntitySelection(hit.transform.parent.gameObject);
            }
        }
        else
        {
            Vector2Int isoGridPosition = MapUtil.ScreenToIsoGrid(Input.mousePosition);

            bool inVerticalBounds = Math.Abs(isoGridPosition.x) <= MapInfo.MapSize;
            bool inHorizontalBounds = Math.Abs(isoGridPosition.y) <= MapInfo.MapSize;

            bool onMap = inVerticalBounds && inHorizontalBounds;

            if (onMap)
            {
                BroadcastCellSelection(MapUtil.ScreenToIsoGrid(Input.mousePosition));
            }
        }
    }
}
