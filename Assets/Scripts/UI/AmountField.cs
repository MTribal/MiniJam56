using TMPro;
using UnityEngine;

public class AmountField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _amountText = default;

    private void OnEnable()
    {
        GameManager.OnMoneyAmountChanged += Atualize;
    }
    
    private void OnDisable()
    {
        GameManager.OnMoneyAmountChanged -= Atualize;
    }

    private void Atualize(int amount)
    {
        _amountText.text = amount.ToString();
    }
}
