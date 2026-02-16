using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.0f;

    [Header("Detection")]
    [SerializeField] private LayerMask playerHitboxMask; // set to PlayerHitbox layer

    private float nextAttackTime;

    private void Update()
    {
        if (Time.time < nextAttackTime) return;

        // Find player hurtbox in range
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerHitboxMask);
        if (hits.Length == 0) return;

        // Damage the first hurtbox we found
        var hurtbox = hits[0].GetComponent<PlayerHurtbox>();
        if (hurtbox == null) return;

        nextAttackTime = Time.time + attackCooldown;
        hurtbox.ApplyDamage(attackDamage);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
