using UnityEngine;


public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRange = 1.6f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1.0f;

    [Header("Target")]
    [SerializeField] private Transform target;
    private Health targetHealth;

    private float nextAttackTime;

    private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        if (target != null)
            targetHealth = target.GetComponent<Health>();
    }

    private void Update()
    {
        if (target == null || targetHealth == null) return;
        if (targetHealth.IsDead) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist <= attackRange && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            targetHealth.TakeDamage(attackDamage);
        }
    }
}

