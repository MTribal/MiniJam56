using UnityEngine;
using My_Utils;

public class TileSlotManager : SingletonScene<TileSlotManager>
{
    [SerializeField] private int _qttOfRanks = default;
    [SerializeField] private float _tileSize = default;
    [SerializeField] private LayerMask _tileSlotLayer = default;

    public int QttOfRanks { get => _qttOfRanks; }
    public int HigherRank { get => _qttOfRanks - 1; }
    public int LowerRank { get => 0; }
    public float TileSize { get => _tileSize; }

    public TileSlot GetTileSlot(Vector2 position)
    {
        Collider2D[] tileSlot = Physics2D.OverlapBoxAll(position, new Vector2(0.7f, 0.7f), 0, _tileSlotLayer);
        return tileSlot[0].GetComponentInChildren<TileSlot>();
    }

    public Rank GetRank(Vector2 position)
    {
        return GetTileSlot(position).GetComponentInParent<Rank>();
    }
}
