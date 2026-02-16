using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    private bool subscribed;

    private void Awake()
    {
        if (moneyText == null) moneyText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        TryHook();
    }

    private void OnEnable()
    {
        TryHook();
    }

    private void OnDisable()
    {
        Unhook();
    }

    private void TryHook()
    {
        if (subscribed) return;
        if (CurrencyManager.Instance == null) return;

        CurrencyManager.Instance.OnMoneyChanged += HandleMoneyChanged;
        subscribed = true;

        HandleMoneyChanged(CurrencyManager.Instance.Money);
    }

    private void Unhook()
    {
        if (!subscribed) return;
        if (CurrencyManager.Instance != null)
            CurrencyManager.Instance.OnMoneyChanged -= HandleMoneyChanged;

        subscribed = false;
    }

    private void HandleMoneyChanged(int newMoney)
    {
        moneyText.text = $"Money: {newMoney}";
    }
}
