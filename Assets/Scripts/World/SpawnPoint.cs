using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Gizmo")]
    public float gizmoRadius = 0.5f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, gizmoRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.5f);
    }
}
