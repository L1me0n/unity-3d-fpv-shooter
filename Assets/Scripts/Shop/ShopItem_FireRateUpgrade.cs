using UnityEngine;

public class ShopItem_FireRateUpgrade : MonoBehaviour
{
    [SerializeField] private int cost = 12;
    [SerializeField] private float multiplier = 1.20f; // 20% faster

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

        playerWeapon.MultiplyFireRate(multiplier);
        Debug.Log($"[SHOP] Fire rate upgraded x{multiplier:F2} ✅");
    }
}
