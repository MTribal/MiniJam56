using UnityEngine;

public class TileSlotManager : MonoBehaviour
{
    [SerializeField] private int _qttOfRanks = default;
    [SerializeField] private float _tileSize = default;
    [SerializeField] private LayerMask _tileSlotLayer = default;

    public static TileSlotManager Instance { get; private set; }
    public int QttOfRanks { get => _qttOfRanks; }
    public int HigherRank { get => _qttOfRanks - 1; }
    public int LowerRank { get => 0; }
    public float TileSize { get => _tileSize; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

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
