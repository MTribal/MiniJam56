using UnityEngine;
using My_Utils;

public class TileSlot : SlotContainer
{
    [SerializeField] private LayerMask _doughnutLayer = default;

    private bool _occupied;

    public void Disoccupe()
    {
        _occupied = false;
    }

    protected override void DroppedObject(GameObject droppedObject)
    {
        if (!_occupied && droppedObject.TryGetComponent(out DragableCard dragableCard))
        {
            _occupied = true;
            dragableCard.PutDoughnut(transform.position);
        }
        else if (_occupied && droppedObject.TryGetComponent(out FatBoy fatBoy))
        {
            _occupied = false;
            Collider2D[] doughnuts = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.5f), 0, _doughnutLayer);
            fatBoy.RemoveDoughnut(doughnuts[0].GetComponent<Doughnut>());
        }
    }
}
