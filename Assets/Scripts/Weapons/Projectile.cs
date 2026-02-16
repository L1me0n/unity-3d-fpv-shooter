using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 4f;

    private float damage;
    private Vector3 velocity;
    private LayerMask hitMask;
    private Transform ownerRoot;

    // Colliders to ignore (shooter body etc.)
    private Collider[] ownerColliders;

    public void Init(float dmg, Vector3 vel, LayerMask mask, Transform owner)
    {
        damage = dmg;
        velocity = vel;
        hitMask = mask;
        ownerRoot = owner;

        if (ownerRoot != null)
        {
            ownerColliders = ownerRoot.GetComponentsInChildren<Collider>(true);

            // Ignore owner collisions
            Collider myCol = GetComponent<Collider>();
            if (myCol != null && ownerColliders != null)
            {
                foreach (var c in ownerColliders)
                {
                    if (c != null) Physics.IgnoreCollision(myCol, c, true);
                }
            }
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // ignore owner
        if (ownerRoot != null && other.transform.IsChildOf(ownerRoot))
            return;

        // layer mask filter
        if (((1 << other.gameObject.layer) & hitMask) == 0)
            return;

        // player hurtbox
        var hurtbox = other.GetComponent<PlayerHurtbox>();
        if (hurtbox != null)
        {
            hurtbox.ApplyDamage(damage);
            Destroy(gameObject);
            return;
        }

        // fallback: anything with Health
        var health = other.GetComponentInParent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // hit something else: destroy anyway
        Destroy(gameObject);
    }
}
