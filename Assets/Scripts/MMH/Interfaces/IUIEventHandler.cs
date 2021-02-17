using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MMH.Handler
{
    public interface IUserInterfaceEventHandler : IEventSystemHandler
    {
        void SelectEntity(GameObject entityObject);
        void SelectCell(int2 cellPosition);

        void ClearSelection();
    }
}