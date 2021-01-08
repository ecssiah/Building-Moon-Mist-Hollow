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
                bool isTarget = hit.collider.gameObject.CompareTag("Target");

                if (isTarget)
                {
                    BroadcastEntitySelection(hit.transform.parent.gameObject);
                }
            }
            else
            {
                Vector2Int isoGridPosition = Util.Map.ScreenToIsoGrid(Input.mousePosition);

                bool inVerticalBounds = Math.Abs(isoGridPosition.x) <= Info.Map.Size;
                bool inHorizontalBounds = Math.Abs(isoGridPosition.y) <= Info.Map.Size;

                bool onMap = inVerticalBounds && inHorizontalBounds;

                if (onMap)
                {
                    BroadcastCellSelection(isoGridPosition);
                }
            }
        }
    }
}