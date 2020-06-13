using UnityEngine;
using UnityEngine.EventSystems;

public class TileSlot : MonoBehaviour, IDropHandler
{
    private bool _occupied;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out DragableCard dragableCard))
        {
            if (!_occupied)
            {
                _occupied = true;
                dragableCard.PutDoughnut(transform.position);
            }
        }
    }
}
