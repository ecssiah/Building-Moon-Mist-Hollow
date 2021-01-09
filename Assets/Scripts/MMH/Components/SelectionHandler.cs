using System;
using UnityEngine;

namespace MMH
{
    public class SelectionHandler : MonoBehaviour
    {
        public Action<GameObject> BroadcastEntitySelection;
        public Action<Vector2Int> BroadcastCellSelection;


        public void Select()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                BroadcastEntitySelection(hit.transform.gameObject);
            }
            else
            {
                Vector2Int isoGridPosition = Util.Map.ScreenToIsoGrid(Input.mousePosition);

                if (Util.Map.OnMap(isoGridPosition))
                {
                    BroadcastCellSelection(isoGridPosition);
                }
            }
        }
    }
}