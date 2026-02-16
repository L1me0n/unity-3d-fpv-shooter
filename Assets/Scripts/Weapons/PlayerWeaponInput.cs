using UnityEngine;

public class PlayerWeaponInput : MonoBehaviour
{
    [SerializeField] private WeaponController weapon;

    private void Update()
    {
        if (ShopManager.Instance != null && ShopManager.Instance.IsOpen)
            return;

        if (!weapon) return;

        if (Input.GetMouseButton(0))
            weapon.TryFire();

        if (Input.GetKeyDown(KeyCode.R))
            weapon.TryReload();
    }
}
