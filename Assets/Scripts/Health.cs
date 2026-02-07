using UnityEngine;
using UnityEngine.Events;

namespace ArenaSurvival.Combat
{
    public class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        [Header("Events")]
        public UnityEvent onDamaged;
        public UnityEvent onDied;

        public float MaxHealth => maxHealth;
        public float CurrentHealth => currentHealth;

        private bool isDead;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void ResetHealth()
        {
            isDead = false;
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            if (isDead) return;
            if (amount <= 0f) return;

            currentHealth -= amount;
            onDamaged?.Invoke();

            if (currentHealth <= 0f)
            {
                currentHealth = 0f;
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            onDied?.Invoke();

            // Phase 2: simplest death = destroy
            Destroy(gameObject);
        }
    }
}
