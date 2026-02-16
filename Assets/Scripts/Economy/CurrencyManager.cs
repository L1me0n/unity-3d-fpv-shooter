using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [Header("State")]
    [SerializeField] private int money = 0;

    public int Money => money;

    public event Action<int> OnMoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ResetMoney(int startAmount = 0)
    {
        money = Mathf.Max(0, startAmount);
        OnMoneyChanged?.Invoke(money);
    }

    public void Add(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(money);
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0) return true;
        if (money < amount) return false;

        money -= amount;
        OnMoneyChanged?.Invoke(money);
        return true;
    }
}
