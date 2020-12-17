using System;
using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public Action<Vector2Int> BroadcastCellSelection;
    public Action<GameObject> BroadcastEntitySelection;


    public void Select()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Target"))
        {
            BroadcastEntitySelection(hit.transform.parent.gameObject);
        }
        else
        {
            BroadcastCellSelection(MapUtil.ScreenToIsoGrid(Input.mousePosition));
        }
    }
}
