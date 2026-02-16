using System;
using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public event Action<int, int> OnAmmoChanged; // current, magSize
    public event Action<bool> OnReloadStateChanged;

    [Header("Setup")]
    [SerializeField] private WeaponDefinition def;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Camera aimCamera;      // for player hitscan aim
    [SerializeField] private LayerMask hitMask;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    private int ammoInMag;
    private bool isReloading;
    private float nextShotTime;

    public WeaponDefinition Definition => def;
    public int AmmoInMag => ammoInMag;
    public bool IsReloading => isReloading;

    private void Awake()
    {
        if (def == null)
        {
            Debug.LogError($"[{name}] WeaponController missing WeaponDefinition (def).");
            enabled = false;
            return;
        }

        if (!audioSource) audioSource = GetComponent<AudioSource>(); // auto find ok

        ammoInMag = def.magazineSize;
        BroadcastAmmo();
    }


    public void SetDefinition(WeaponDefinition newDef)
    {
        def = newDef;
        ammoInMag = def.magazineSize;
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

        nextShotTime = Time.time + (1f / def.fireRate);
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
        if (ammoInMag == def.magazineSize) return;

        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        OnReloadStateChanged?.Invoke(true);

        if (def.reloadSfx && audioSource) audioSource.PlayOneShot(def.reloadSfx);

        yield return new WaitForSeconds(def.reloadTime);

        ammoInMag = def.magazineSize;
        BroadcastAmmo();

        isReloading = false;
        OnReloadStateChanged?.Invoke(false);
    }

    private void FireHitscan()
    {
        // Player uses camera aim. Enemy can set aimCamera to a "fake" camera
        // OR we can later add an alternative hitscan direction mode.
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
    }



    private void BroadcastAmmo()
    {
        OnAmmoChanged?.Invoke(ammoInMag, def.magazineSize);
    }
}
