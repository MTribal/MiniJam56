using System;
using UnityEngine;

public delegate void ReceiveAmount(int amount); 

public class GameManager : SingletonPermanent<GameManager>
{
    public bool t_debug;

    [Tooltip("The amount of money that player starts with.")]
    [SerializeField] private int _initialMoney = default;

    /// <summary>
    /// Called when atual money amount changed.
    /// </summary>
    public static event ReceiveAmount OnMoneyAmountChanged;

    /// <summary>
    /// The atual money amount.
    /// </summary>
    public int MoneyAmount 
    {
        get
        {
            return _moneyAmount;
        }

        private set
        {
            _moneyAmount = value;
            OnMoneyAmountChanged?.Invoke(_moneyAmount);
        }
    }
    private int _moneyAmount;

    private void Start()
    {
        MoneyAmount = _initialMoney;

        if (t_debug)
        {

        }
    }

    public void Withdraw(int amount)
    {
        MoneyAmount -= amount;
    }

    public void Deposit(int amount)
    {
        MoneyAmount += amount;
    }
}
