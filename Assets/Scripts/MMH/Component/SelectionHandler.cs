using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMH
{
    public class SelectionHandler : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Select();
            }
        }


        private void Select()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                ExecuteEvents.Execute<Handler.ISelectionHandler>(
                    gameObject, null, (x, y) => x.SelectEntity(hit.transform.gameObject)
                );
            }
            else
            {
                Vector2Int isoGridPosition = Util.Map.ScreenToIsoGrid(Input.mousePosition);

                if (Util.Map.OnMap(isoGridPosition))
                {
                    ExecuteEvents.Execute<Handler.ISelectionHandler>(
                        gameObject, null, (x, y) => x.SelectCell(isoGridPosition)
                    );
                }
            }
        }
    }
}