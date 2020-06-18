using UnityEngine;

public class Rank : MonoBehaviour
{
    [SerializeField] private int _rank = default;

    /// <summary>
    /// The rank number [0 - 4]. Higher numbers are higher ranks.
    /// </summary>
    public int RankNumber { get => _rank; }
}
