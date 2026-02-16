using TMPro;
using UnityEngine;

public class AmmoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private WeaponController weapon;

    private void OnEnable()
    {
        if (weapon != null) weapon.OnAmmoChanged += HandleAmmo;
    }

    private void OnDisable()
    {
        if (weapon != null) weapon.OnAmmoChanged -= HandleAmmo;
    }

    private void Start()
    {
        if (weapon != null)
            HandleAmmo(weapon.AmmoInMag, weapon.Definition.magazineSize);
    }

    private void HandleAmmo(int current, int magSize)
    {
        ammoText.text = $"{current} / {magSize}";
    }
}
