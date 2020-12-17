using System;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public Action<GameObject> BroadcastEntitySelection;
    public Action<Vector3Int> BroadcastCellSelection;


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
            BroadcastCellSelection(MapUtil.ScreenToIsoGrid(Input.mousePosition));
        }
    }
}
