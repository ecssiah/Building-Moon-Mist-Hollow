using UnityEngine;
using UnityEngine.EventSystems;

namespace MMH.Handler
{
    public interface IUIEventHandler : IEventSystemHandler
    {
        void SelectEntity(GameObject entityObject);
        void SelectCell(Vector2Int cellPosition);

        void ClearSelection();
    }
}