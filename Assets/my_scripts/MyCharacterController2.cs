using UnityEngine;
using UnityEngine.InputSystem;

public class MyCharacterController2 : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 15f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float pushPower = 2.0f; // For pushing cubes

    [Header("Camera References")]
    public GameObject fpsCamera;
    public GameObject tpsCamera;
    public Transform cameraHolder;

    // Internal Variables
    private CharacterController controller; // The new component
    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private bool isFpsMode = true;
    private Vector3 velocity; // Stores our falling speed
    private bool isGrounded;

    void Awake()
    {
        // Get the CharacterController component instead of Rigidbody
        controller = GetComponent<CharacterController>();
        controls = new PlayerControls();

        // Setup Input
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => lookInput = Vector2.zero;

        controls.Gameplay.SwitchCamera.performed += ctx => ToggleCamera();
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    void Update()
    {
        // CharacterController movement usually goes in Update, not FixedUpdate
        HandleRotation();
        HandleMovement();
    }

    void HandleMovement()
    {
        // 1. Check if we are on the ground
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep us stuck to floor
        }

        // 2. Move Player (WASD)
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 3. Apply Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleRotation()
    {
        // 1. Rotate Player (Y-axis)
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // 2. Rotate Camera (X-axis)
        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void ToggleCamera()
    {
        isFpsMode = !isFpsMode;
        fpsCamera.SetActive(isFpsMode);
        tpsCamera.SetActive(!isFpsMode);
    }

    // BONUS: This function allows CharacterController to push Rigidbodies
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // No rigidbody or it's kinematic? Do nothing.
        if (body == null || body.isKinematic) return;

        // Don't push objects down (prevent walking on top of them)
        if (hit.moveDirection.y < -0.3f) return;

        // Calculate push direction from move direction,
        // we only push objects to the sides, never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // Apply the push
        body.linearVelocity = pushDir * pushPower; 
        // Note: Use body.velocity if linearVelocity gives an error
    }
}