using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2.0f;
    public float lookUpLimit = -80.0f;
    public float lookDownLimit = 80.0f;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private float verticalRotation = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        // Check if the player is touching the ground
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            // Reset velocity but keep a tiny downward force to stay snapped to the ground
            velocity.y = -2f;
        }

        // Get movement inputs (WASD / Arrow Keys)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Calculate move direction relative to the direction the player is facing
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Jump logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Physics formula for calculating exact jump velocity based on desired height
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Apply constant gravity over time
        velocity.y += gravity * Time.deltaTime;

        // Apply final vertical velocity (gravity/jumping) to the character
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleLook()
    {
        // Get mouse movements
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the body horizontally (Left / Right)
        transform.Rotate(Vector3.up * mouseX);

        // Calculate and clamp vertical rotation (Up / Down) to prevent flipping upside down
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, lookUpLimit, lookDownLimit);

        // Apply the rotation strictly to the attached camera object
        if (cameraTransform != null)
        {
            cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        }
    }
}

