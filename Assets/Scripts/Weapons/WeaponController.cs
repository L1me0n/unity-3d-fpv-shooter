using System;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public event Action<int, int> OnAmmoChanged; // current, magSize
    public event Action<bool> OnReloadStateChanged;

    [Header("Setup")]
    [SerializeField] private WeaponDefinition def;
    [SerializeField] private WeaponView weaponView;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Camera aimCamera;      // for player hitscan aim
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private int bonusMagSize = 0;
    [SerializeField] private float fireRateMultiplier = 1f; // 1 = normal, 1.25 = 25% faster

    [SerializeField] private AudioClip reloadSfx;
    [SerializeField] private AudioClip shootSfx;
    [SerializeField] private float shootVolume = 0.9f;



    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    private int ammoInMag;
    private bool isReloading;
    private float nextShotTime;

    public WeaponDefinition Definition => def;
    public int AmmoInMag => ammoInMag;
    public bool IsReloading => isReloading;
    public int CurrentMagSize => def.magazineSize + bonusMagSize;

    private void Awake()
    {
        if (def == null)
        {
            Debug.LogError($"[{name}] WeaponController missing WeaponDefinition (def).");
            enabled = false;
            return;
        }

        if (!audioSource) audioSource = GetComponent<AudioSource>();

        ammoInMag = CurrentMagSize;
        BroadcastAmmo();
    }


    public void SetDefinition(WeaponDefinition newDef)
    {
        def = newDef;
        ammoInMag = CurrentMagSize;
        isReloading = false;
        nextShotTime = 0f;
        BroadcastAmmo();
    }

    public void TryFire()
    {
        if (def == null) return;
        if (isReloading) return;
        if (Time.time < nextShotTime) return;

        if (ammoInMag <= 0)
        {
            // auto-reload if empty
            TryReload();
            return;
        }

        nextShotTime = Time.time + (1f / CurrentFireRate);
        ammoInMag--;
        BroadcastAmmo();

        if (def.fireType == FireType.Hitscan)
            FireHitscan();
        else
            FireProjectile();

        if (def.shootSfx && audioSource) audioSource.PlayOneShot(def.shootSfx);
    }

    public void TryReload()
    {
        if (def == null) return;
        if (isReloading) return;
        if (ammoInMag == CurrentMagSize) return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        OnReloadStateChanged?.Invoke(true);

        if (def.reloadSfx && audioSource) audioSource.PlayOneShot(def.reloadSfx);

        yield return new WaitForSeconds(def.reloadTime);

        ammoInMag = CurrentMagSize;
        BroadcastAmmo();

        isReloading = false;
        OnReloadStateChanged?.Invoke(false);
        AudioManager.Instance?.Play2D(reloadSfx, 0.9f, 0.98f, 1.02f);
    }

    private void FireHitscan()
    {
        // Player uses camera aim. Enemy can set aimCamera to a "fake" camera
        Vector3 origin = aimCamera ? aimCamera.transform.position : muzzle.position;
        Vector3 direction = aimCamera ? aimCamera.transform.forward : muzzle.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, def.range, hitMask))
        {
            var health = hit.collider.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(def.damage);
            }
        }
    }

    private void FireProjectile()
    {
        if (!def.projectilePrefab || !muzzle) return;

        var go = Instantiate(def.projectilePrefab, muzzle.position, muzzle.rotation);
        var proj = go.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(def.damage, muzzle.forward * def.projectileSpeed, hitMask, transform.root);
        }
        AudioManager.Instance?.Play2D(shootSfx, shootVolume, 0.95f, 1.05f);
    }

    private void BroadcastAmmo()
    {
        OnAmmoChanged?.Invoke(ammoInMag, CurrentMagSize);
    }

    public void AddBonusMagSize(int amount)
    {
        bonusMagSize += Mathf.Max(0, amount);

        ammoInMag = Mathf.Min(ammoInMag, CurrentMagSize);
    }

    public float CurrentFireRate
    {
        get
        {
            // assuming WeaponDefinition.fireRate means shots per second
            return def.fireRate * fireRateMultiplier;
        }
    }
    public void MultiplyFireRate(float multiplier)
    {
        // multiplier should be > 1 to increase fire rate
        if (multiplier <= 1f) return;

        fireRateMultiplier *= multiplier;

        fireRateMultiplier = Mathf.Clamp(fireRateMultiplier, 0.1f, 5f);
    }

    public void Equip(WeaponDefinition newDef)
    {
        if (newDef == null) return;

        def = newDef;

        // Reset weapon state safely
        ammoInMag = CurrentMagSize;   // uses upgraded mag size logic
        isReloading = false;
        nextShotTime = 0f;             

        // Refresh view model
        if (weaponView != null)
            weaponView.Apply(newDef);   // method shown below
    }

}
