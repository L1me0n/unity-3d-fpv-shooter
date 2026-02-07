using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float stoppingDistance = 1.6f;

    [Header("Target")]
    [SerializeField] private Transform target;

    private void Start()
    {
        // Auto-find player if not assigned
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f; // stay horizontal, avoid flying up/down

        float distance = toTarget.magnitude;
        if (distance <= stoppingDistance) return;

        Vector3 dir = toTarget.normalized;

        // Move directly toward player
        transform.position += dir * moveSpeed * Time.deltaTime;

        // Face the player
        if (dir.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
}
