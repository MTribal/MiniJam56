using UnityEngine;

public class GenerateCards : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private CardData[] _cards;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for (int i = 0; i < _cards.Length; i++)
        {
            GameObject cardPrefab = Instantiate(_cardPrefab, transform);
            cardPrefab.name = "Card_" + i;
            cardPrefab.GetComponentInChildren<BaseCard>().SetCardData(_cards[i]);
            cardPrefab.GetComponentInChildren<DragableCard>().SetCardData(_cards[i]);
        }
    }
}
