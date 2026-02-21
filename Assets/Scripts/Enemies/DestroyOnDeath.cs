using UnityEngine;


public class DestroyOnDeath : MonoBehaviour
{
    [SerializeField] private Health health;

    [SerializeField] private AudioClip dieSfx;

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
        AudioManager.Instance?.Play2D(dieSfx, 0.7f, 0.98f, 1.02f);
    }
}

