using UnityEngine;

public class EnemySquareShooterBrain : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;
    [SerializeField] private CharacterController cc;
    [SerializeField] private WeaponController weapon;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float gravity = -18f;

    [Header("Combat")]
    [SerializeField] private float stopDistance = 12f;  // start shooting here
    [SerializeField] private float chaseDistance = 18f; // if player is far, chase again
    [SerializeField] private Transform aimPivot;        // optional: where we rotate

    private float yVel;

    private void Awake()
    {
        if (!cc) cc = GetComponent<CharacterController>();
        if (!weapon) weapon = GetComponent<WeaponController>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    private void Update()
    {
        if (!player) return;

        // gravity
        if (cc.isGrounded && yVel < 0f) yVel = -2f;
        yVel += gravity * Time.deltaTime;

        float dist = Vector3.Distance(transform.position, player.position);

        // face player (flat rotation)
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;
        if (toPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(toPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 10f * Time.deltaTime);
        }

        // movement logic
        Vector3 move = Vector3.zero;

        if (dist > chaseDistance)
        {
            // chase
            Vector3 dir = (player.position - transform.position);
            dir.y = 0f;
            dir.Normalize();
            move = dir * moveSpeed;
        }
        else if (dist < stopDistance)
        {
            // back up slightly, or just stop
            move = Vector3.zero;
        }
        else
        {
            // in shooting zone: stop
            move = Vector3.zero;
        }

        move.y = yVel;
        cc.Move(move * Time.deltaTime);

        // shooting: only when inside chaseDistance
        if (dist <= chaseDistance)
        {
            weapon.TryFire();
        }
    }

    public void SetPlayer(Transform p) => player = p;
}
