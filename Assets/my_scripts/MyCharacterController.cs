using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; // Needed for the UI Text
using System.Collections; // Needed for the Timer (Coroutine)

public class MyCharacterController : MonoBehaviour
{
    [Header("UI Settings")]
    public TextMeshProUGUI saveMessageText; // Drag your Text (TMP) object here

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float lookSensitivity = 15f; 

    [Header("Camera References")]
    public GameObject fpsCamera;
    public GameObject tpsCamera;
    public Transform cameraHolder;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;      // Drag your Bullet Prefab here
    public Transform firePoint;          // The position where bullets spawn
    public float bulletSpeed = 20f;
    public SimpleAudioManager audioManager; // Drag the AudioManager object here

    // Private variables
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

        // --- Setup Input Actions ---
        controls.Gameplay.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.Look.canceled += ctx => lookInput = Vector2.zero;

        controls.Gameplay.SwitchCamera.performed += ctx => ToggleCamera();
    }

    void Start()
    {
        // clear the "Game Saved" text when the game starts so it's invisible
        if (saveMessageText != null)
        {
            saveMessageText.text = "";
        }
    }

    void OnEnable() => controls.Gameplay.Enable();
    void OnDisable() => controls.Gameplay.Disable();

    void Update()
    {
        HandleRotation();

        // --- SHOOTING (Press R) ---
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Shoot();
        }

        // --- SAVE GAME (Press F5) ---
        if (Keyboard.current.f5Key.wasPressedThisFrame)
        {
            SaveSystem.SavePlayer(this);       // 1. Save the JSON file
            StartCoroutine(ShowSaveMessage()); // 2. Show the "Saved!" UI
        }

        // --- LOAD GAME (Press F9) ---
        if (Keyboard.current.f9Key.wasPressedThisFrame)
        {
            LoadGame();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // --- MOVEMENT LOGIC ---
    void HandleMovement()
    {
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        
        Vector3 targetVelocity = movement * moveSpeed;
        targetVelocity.y = rb.linearVelocity.y; // Keep existing gravity (Unity 6)
        
        rb.linearVelocity = targetVelocity; 
    }

    // --- ROTATION LOGIC ---
    void HandleRotation()
    {
        // Rotate body Left/Right
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera Up/Down
        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    // --- CAMERA SWITCHING ---
    void ToggleCamera()
    {
        isFpsMode = !isFpsMode;
        fpsCamera.SetActive(isFpsMode);
        tpsCamera.SetActive(!isFpsMode);
    }

    // --- SHOOTING LOGIC ---
    void Shoot()
    {
        // 1. Play Sound
        if (audioManager != null)
        {
            audioManager.PlayShootSound();
        }

        // 2. Spawn Bullet
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            
            // 3. Add Force
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = firePoint.forward * bulletSpeed;
            }

            // Cleanup: Destroy bullet after 3 seconds
            Destroy(bullet, 3f);
        }
    }

    // --- LOADING LOGIC ---
    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (data != null)
        {
            // Apply Position
            Vector3 loadedPos;
            loadedPos.x = data.position[0];
            loadedPos.y = data.position[1];
            loadedPos.z = data.position[2];
            transform.position = loadedPos;

            // Apply Rotation
            Quaternion loadedRot;
            loadedRot.x = data.rotation[0];
            loadedRot.y = data.rotation[1];
            loadedRot.z = data.rotation[2];
            loadedRot.w = data.rotation[3];
            transform.rotation = loadedRot;

            Debug.Log("Player State Loaded!");
        }
    }

    // --- UI TIMER LOGIC ---
    IEnumerator ShowSaveMessage()
    {
        if (saveMessageText != null)
        {
            saveMessageText.text = "GAME SAVED!"; // Show text
            yield return new WaitForSeconds(2f);  // Wait 2 seconds
            saveMessageText.text = "";            // Clear text
        }
    }
}