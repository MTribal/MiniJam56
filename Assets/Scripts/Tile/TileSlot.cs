using UnityEngine;
using UnityEngine.EventSystems;

public class TileSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private LayerMask _doughnutLayer = default;
    
    private bool _occupied;

    public void Disoccupe()
    {
        _occupied = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (!_occupied && eventData.pointerDrag.TryGetComponent(out DragableCard dragableCard))
            {
                _occupied = true;
                dragableCard.PutDoughnut(transform.position);
            }
            else if (_occupied && eventData.pointerDrag.TryGetComponent(out FatBoy fatBoy))
            {
                _occupied = false;
                Collider2D[] doughnuts = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0, _doughnutLayer);
                fatBoy.RemoveDoughnut(doughnuts[0].GetComponent<Doughnut>());
            }
        }
    }
}
