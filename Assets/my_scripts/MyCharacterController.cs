using UnityEngine;
using UnityEngine.InputSystem; // Need this for the New Input System

public class MyCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 15f; // Mouse speed

    [Header("Camera References")]
    public GameObject fpsCamera;
    public GameObject tpsCamera;
    public Transform cameraHolder; // The empty object holding cameras

    // Internal Variables
    private Rigidbody rb;
    private PlayerControls controls; // The Input System class we generated
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private bool isFpsMode = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        // Setup Input Listeners
        // When we press WASD, store the value
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        // When we move mouse, store the value
        controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => lookInput = Vector2.zero;

        // When we press C, switch camera
        controls.Gameplay.SwitchCamera.performed += ctx => ToggleCamera();
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    void Update()
    {
        HandleRotation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Calculate movement direction relative to where we are looking
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        // Move the Rigidbody physically
        // We preserve the current Y velocity so gravity still works
        Vector3 targetVelocity = movement * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; // Note: Use rb.velocity in older Unity versions, linearVelocity in Unity 6
        
        rb.linearVelocity = targetVelocity; 
    }

    void HandleRotation()
    {
        // 1. Rotate the Player Left/Right (Y-axis)
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // 2. Rotate the Camera Up/Down (X-axis)
        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Stop head from spinning 360

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void ToggleCamera()
    {
        isFpsMode = !isFpsMode;
        
        // Simple switch: Turn one on, turn the other off
        fpsCamera.SetActive(isFpsMode);
        tpsCamera.SetActive(!isFpsMode);
    }
}