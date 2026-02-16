using UnityEngine;

public class ShopItem_MagUpgrade : MonoBehaviour
{
    [SerializeField] private int cost = 10;
    [SerializeField] private int magIncrease = 6;

    [Header("References")]
    [SerializeField] private WeaponController playerWeapon;

    public void Buy()
    {
        if (CurrencyManager.Instance == null) return;
        if (playerWeapon == null)
        {
            Debug.LogError("[SHOP] playerWeapon not assigned!");
            return;
        }

        if (!CurrencyManager.Instance.TrySpend(cost))
        {
            Debug.Log("[SHOP] Not enough money ❌");
            return;
        }

        playerWeapon.AddBonusMagSize(magIncrease);
        Debug.Log($"[SHOP] Magazine upgraded +{magIncrease} ✅");
    }
}
