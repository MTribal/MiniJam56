using UnityEngine;

public class GenerateCards : MonoBehaviour
{
    [SerializeField] private Card _cardPrefab = default;
    [SerializeField] private CardData[] _cards = default;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            Card cardPrefab = Instantiate(_cardPrefab, transform);
            cardPrefab.name = "Card_" + i;
            cardPrefab.StaticCard.Initialize(_cards[i]);
            cardPrefab.DragableCard.Initialize(_cards[i]);
        }
    }
}
