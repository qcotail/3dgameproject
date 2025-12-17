using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public Camera mainCamera;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // Get camera's forward and right directions (flattened to ball's surface)
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        
        // Remove component pointing toward/away from planet
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        
        // Calculate movement relative to camera
        Vector3 movement = (forward * moveVertical + right * moveHorizontal);
        
        rb.AddForce(movement * speed);
    }
}