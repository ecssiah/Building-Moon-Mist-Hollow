using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMH.Handler
{
    public interface ISelectionHandler : IEventSystemHandler
    {
        void SelectEntity(GameObject entityObject);
        void SelectCell(Vector2Int cellPosition);
        void ClearSelection();
    }
}