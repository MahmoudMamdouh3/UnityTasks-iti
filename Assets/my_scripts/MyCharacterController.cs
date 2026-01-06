using UnityEngine;
using UnityEngine.InputSystem; 

public class MyCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 15f; 

    [Header("Camera References")]
    public GameObject fpsCamera;
    public GameObject tpsCamera;
    public Transform cameraHolder;

   
    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private bool isFpsMode = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

 
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
        HandleRotation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        
    
        Vector3 targetVelocity = movement * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; 
        
        rb.linearVelocity = targetVelocity; 
    }

    void HandleRotation()
    {
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

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
}