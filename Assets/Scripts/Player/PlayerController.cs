using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot; // assign PlayerCamera

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 80f;

    private CharacterController controller;
    private float verticalVelocity;
    private float pitch; // up/down camera rotation

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        // Lock cursor for FPS feel
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (ShopManager.Instance != null && ShopManager.Instance.IsOpen)
            return;

        HandleLook();
        HandleMove();
    }

    private void HandleMove()
    {
        float x = Input.GetAxis("Horizontal"); // A/D
        float z = Input.GetAxis("Vertical");   // W/S

        Vector3 move = (transform.right * x + transform.forward * z) * moveSpeed;

        // Gravity
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // keeps player grounded

        verticalVelocity += gravity * Time.deltaTime;

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // rotate player body left/right
        transform.Rotate(Vector3.up * mouseX);

        // rotate camera up/down (pitch)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
