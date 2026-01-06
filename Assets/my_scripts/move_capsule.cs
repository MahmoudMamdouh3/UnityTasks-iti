using UnityEngine;

public class move_capsule : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
      
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

      
        Vector3 movement = new Vector3(moveX, 0, moveZ);

   
        rb.linearVelocity = movement * moveSpeed; 

    }
}