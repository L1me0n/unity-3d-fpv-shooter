using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyChase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float stoppingDistance = 1.2f;

    [Header("Target")]
    [SerializeField] private Transform target;

    private CharacterController cc;
    private float verticalVelocity;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    private void Update()
    {
        if (target == null) return;

        // --- Horizontal move toward player ---
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;

        Vector3 move = Vector3.zero;

        if (toTarget.magnitude > stoppingDistance)
        {
            Vector3 dir = toTarget.normalized;
            move = dir * moveSpeed;

            // face direction
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.LookRotation(dir);
        }

        // --- Gravity ---
        if (cc.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // small downward force to "stick" to ground
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 finalMove = (move + Vector3.up * verticalVelocity) * Time.deltaTime;
        cc.Move(finalMove);
    }
}
