using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    private void Awake()
    {
        if (!playerHealth)
            playerHealth = GetComponentInParent<Health>();

        if (!playerHealth)
            Debug.LogError("[PlayerHurtbox] No Health found in parents.");
    }

    public void ApplyDamage(float dmg)
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(dmg);
    }
}
