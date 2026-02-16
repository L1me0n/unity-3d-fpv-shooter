using UnityEngine;

public class ShopItem_BuyWeapon : MonoBehaviour
{
    [SerializeField] private int cost = 25;
    [SerializeField] private WeaponDefinition weaponToBuy;
    [SerializeField] private WeaponController playerWeapon;

    private bool purchased;

    public void Buy()
    {
        if (weaponToBuy == null)
        {
            Debug.LogError("[SHOP] weaponToBuy not assigned!");
            return;
        }
        if (playerWeapon == null)
        {
            Debug.LogError("[SHOP] playerWeapon not assigned!");
            return;
        }
        if (purchased)
        {
            // Already owned: just equip for free
            playerWeapon.Equip(weaponToBuy);
            Debug.Log("[SHOP] Equipped owned weapon ✅");
            return;
        }

        if (CurrencyManager.Instance == null) return;

        if (!CurrencyManager.Instance.TrySpend(cost))
        {
            Debug.Log("[SHOP] Not enough money ❌");
            return;
        }

        purchased = true;
        playerWeapon.Equip(weaponToBuy);
        Debug.Log($"[SHOP] Bought + equipped weapon for ${cost} ✅");
    }
}
