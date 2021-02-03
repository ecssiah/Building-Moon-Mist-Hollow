using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMH.Component
{
    public class SelectionHandler : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PrimarySelect();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SecondarySelect();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }


        private void PrimarySelect()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                ExecuteEvents.Execute<Handler.IUIEventHandler>(
                    gameObject, null, (x, y) => x.SelectEntity(hit.transform.gameObject)
                );
            }
            else
            {
                int2 isoGridPosition = Util.Map.ScreenToIsoGrid(new float2(Input.mousePosition.x, Input.mousePosition.y));

                if (Util.Map.OnMap(isoGridPosition))
                {
                    ExecuteEvents.Execute<Handler.IUIEventHandler>(
                        gameObject, null, (x, y) => x.SelectCell(isoGridPosition)
                    );
                }
            }
        }


        private void SecondarySelect()
        {
            Debug.Log("Secondary Selection");
        }


        private void Cancel()
        {
            ExecuteEvents.Execute<Handler.IUIEventHandler>(
                gameObject, null, (x, y) => x.ClearSelection()
            );
        }
    }
}