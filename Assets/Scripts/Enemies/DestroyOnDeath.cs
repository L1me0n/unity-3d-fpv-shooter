using UnityEngine;


public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private Health health;

    private void Awake()
    {
        if (health == null) health = GetComponent<Health>();
        if (health != null) health.onDied.AddListener(HandleDeath);
    }

    private void OnDestroy()
    {
        if (health != null) health.onDied.RemoveListener(HandleDeath);
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}

