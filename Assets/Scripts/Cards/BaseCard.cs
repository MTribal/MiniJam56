using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseCard : MonoBehaviour
{
    [SerializeField] protected CardData _cardData;

    [SerializeField] protected Image _backgroundImage;
    [SerializeField] protected Image _dounughtImage;
    [SerializeField] private TextMeshProUGUI _costText = default;

    public virtual void SetCardData(CardData cardData)
    {
        _cardData = cardData;
        Atualize();
    }

    private void Atualize()
    {
        _backgroundImage.sprite = _cardData.background;
        _dounughtImage.sprite = _cardData.image;
        _costText.text = _cardData.cost.ToString();
    }
}
