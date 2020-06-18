using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage = default;
    [SerializeField] private Image _dounughtImage = default;
    [SerializeField] private TextMeshProUGUI _costText = default;

    public CardData CardData { get; private set; } = default;

    public void Initialize(CardData cardData)
    {
        CardData = cardData;
        Atualize();
    }

    private void Atualize()
    {
        _backgroundImage.sprite = CardData.background;
        _dounughtImage.sprite = CardData.image;
        _costText.text = CardData.cost.ToString();
    }
}
