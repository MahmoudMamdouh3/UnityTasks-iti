using UnityEngine;

public class move_capsule : MonoBehaviour
{
    // This lets you adjust speed inside the Inspector
    public float moveSpeed = 5f;
    
    // Variable to hold the reference to the Rigidbody component
    private Rigidbody rb;

    void Start()
    {
        // 1. Find the Rigidbody attached to this same capsule
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 2. Read the keyboard input (WASD or Arrows)
        // "Horizontal" returns -1 (A/Left), 0 (None), or 1 (D/Right)
        // "Vertical" returns -1 (S/Down), 0 (None), or 1 (W/Up)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 3. Calculate the movement direction
        // We put moveX on the X axis, 0 on Y (we don't want to fly), and moveZ on Z
        Vector3 movement = new Vector3(moveX, 0, moveZ);

        // 4. Move the Rigidbody
        // We set the velocity directly for snappy movement
        rb.linearVelocity = movement * moveSpeed; 
        
        // Note: If you get an error saying 'linearVelocity' does not exist, 
        // change 'rb.linearVelocity' to 'rb.velocity' (Unity 6 updated this naming).
    }
}