using UnityEngine;

[RequireComponent(typeof(Destructible))]
public class ExplosionOnDeath : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float radius = 6f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float force = 900f;
    [SerializeField] private float upwardModifier = 0.4f;

    [Header("Layers")]
    [SerializeField] private LayerMask hitMask = ~0; // everything by default

    [Header("Optional VFX")]
    [SerializeField] private GameObject explosionVfxPrefab;

    private Destructible destructible;

    private void Awake()
    {
        destructible = GetComponent<Destructible>();
        destructible.OnDied += Explode;
    }

    private void OnDestroy()
    {
        if (destructible != null)
            destructible.OnDied -= Explode;
    }

    private void Explode()
    {
        Vector3 pos = transform.position;

        if (explosionVfxPrefab != null)
            Instantiate(explosionVfxPrefab, pos, Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(pos, radius, hitMask, QueryTriggerInteraction.Collide);

        foreach (var col in hits)
        {
            // 1) Physics knockback
            if (col.attachedRigidbody != null)
                col.attachedRigidbody.AddExplosionForce(force, pos, radius, upwardModifier, ForceMode.Impulse);

            var playerHurtbox = col.GetComponentInParent<PlayerHurtbox>();
            if (playerHurtbox != null)
            {
                playerHurtbox.ApplyDamage(damage);
                continue;
            }

            // 2) Damage anything that has Health OR Destructible
            var health = col.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
                continue;
            }

            var otherDestructible = col.GetComponentInParent<Destructible>();
            if (otherDestructible != null && otherDestructible != destructible)
            {
                otherDestructible.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
