using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private BaseCard _staticCard = default;
    [SerializeField] private DragableCard _dragableCard = default;
    
    public BaseCard StaticCard
    {
        get
        {
            return _staticCard;
        }
    }

    public DragableCard DragableCard
    {
        get
        {
            return _dragableCard;
        }
    }
}
