using UnityEngine;




public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Shooting")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float range = 200f;
    [SerializeField] private float fireRate = 8f; // bullets per second
    [SerializeField] private LayerMask hitMask = ~0; // everything by default

    [Header("Debug")]
    [SerializeField] private bool drawDebugRay = true;

    [SerializeField] private HitmarkerUI hitmarker;


    private float nextFireTime;

    private void Reset()
    {
        // Auto-fill camera when added in editor
        playerCamera = GetComponentInParent<Camera>();
    }

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = GetComponentInParent<Camera>();
        }
    }

    private void Update()
    {
        HandleShootInput();
    }

    private void HandleShootInput()
    {
        // Old Input System: Fire1 is left click by default
        bool wantsToShoot = Input.GetButton("Fire1");

        if (!wantsToShoot) return;

        float interval = 1f / fireRate;
        if (Time.time < nextFireTime) return;

        nextFireTime = Time.time + interval;

        Shoot();
    }

    private void Shoot()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (drawDebugRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 0.05f);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            // Try damage
            if (hit.collider.TryGetComponent<Health>(out Health health))
            {
                health.TakeDamage(damage);
                if (hitmarker != null) hitmarker.Show();
            }


        }
    }
}

