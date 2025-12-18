using UnityEngine;

public class SphericalGravity : MonoBehaviour
{
    public Transform planet; // The planet to pull toward
    public float gravity = 9.8f; // Gravity strength
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable default gravity
        //rb.constraints = RigidbodyConstraints.FreezeRotation; // Optional: prevents ball from rotating weirdly
    }
    
    void FixedUpdate()
    {
        // Calculate direction from ball to planet center
        Vector3 gravityDirection = (planet.position - transform.position).normalized;
        
        // Apply force toward planet
        rb.AddForce(gravityDirection * gravity);
        
        // Orient ball to stand upright relative to planet surface
        // Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDirection) * transform.rotation;
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 50 * Time.deltaTime);
    }
}