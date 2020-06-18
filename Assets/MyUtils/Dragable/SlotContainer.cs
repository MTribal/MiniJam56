using My_Utils;
using UnityEngine;
using UnityEngine.EventSystems;


namespace My_Utils
{
    /// <summary>
    /// Base class of all slots containers. Inherit this class to create a new SlotContainer type.
    /// </summary>
    public abstract class SlotContainer : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out IDragable dragable))
            {
                DroppedObject(eventData.pointerDrag);
            }
        }


        /// <summary>
        /// All dropped objects need to implement IDragable.
        /// </summary>
        protected abstract void DroppedObject(GameObject droppedObject);
    }
}
