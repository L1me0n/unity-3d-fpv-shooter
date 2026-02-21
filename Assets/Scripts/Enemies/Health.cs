using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [SerializeField] private AudioClip hurtSfx;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead { get; private set; }

    public UnityEvent onDamaged;
    public UnityEvent onDied;

    private void Awake()
    {
        ResetHealth();
    }

    public void ResetHealth()
    {
        IsDead = false;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        onDamaged?.Invoke();

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
        AudioManager.Instance?.Play2D(hurtSfx, 0.7f, 0.95f, 1.05f);

    }

    private void Die()
    {
        if (IsDead) return;
        IsDead = true;
        onDied?.Invoke();
    }
}

