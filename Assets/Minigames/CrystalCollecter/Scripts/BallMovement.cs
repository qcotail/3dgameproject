using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public Transform planet; // NEW: need planet reference
    
    private Rigidbody rb;
    private Camera mainCamera;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }
    
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        
        // Get planet up direction
        Vector3 planetUp = (transform.position - planet.position).normalized;
        
        // Get camera directions projected onto planet surface
        Vector3 forward = Vector3.ProjectOnPlane(mainCamera.transform.forward, planetUp).normalized;
        Vector3 right = Vector3.ProjectOnPlane(mainCamera.transform.right, planetUp).normalized;
        
        // Calculate movement relative to planet surface
        Vector3 movement = (forward * moveVertical + right * moveHorizontal);
        
        rb.AddForce(movement * speed);
    }
}