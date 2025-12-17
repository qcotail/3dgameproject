using UnityEngine;

public class SimpleBallCamera : MonoBehaviour
{
    public Transform ball;
    public Transform planet;
    public Rigidbody ballRb;
    public float distance = 25f;
    public float height = 15f;
    public float sideOffset = 5f;
    public float smoothSpeed = 5f;
    public float rotationSpeed = 3f;
    
    private Vector3 currentVelocity;
    
    void LateUpdate()
    {
        if (ball == null || ballRb == null || planet == null) return;
        
        // Calculate "up" relative to planet (ball's position away from planet center)
        Vector3 planetUp = (ball.position - planet.position).normalized;
        
        // Get ball's velocity projected onto the planet's surface
        Vector3 velocity = ballRb.velocity;
        Vector3 moveDirection = Vector3.ProjectOnPlane(velocity, planetUp).normalized;
        
        // If ball is moving, position camera behind movement direction
        if (moveDirection.magnitude > 0.1f)
        {
            // Camera position: behind movement, above planet surface, slightly to side
            Vector3 targetPosition = ball.position - moveDirection * distance + planetUp * height + Vector3.Cross(planetUp, moveDirection) * sideOffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / smoothSpeed);
            
            // Look at ball, using planet up as reference
            Vector3 lookDirection = ball.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Ball stopped - maintain position
            Vector3 offset = transform.position - ball.position;
            Vector3 offsetProjected = Vector3.ProjectOnPlane(offset, planetUp).normalized;
            transform.position = ball.position + offsetProjected * distance + planetUp * height;
            
            // Look at ball with planet up
            Quaternion targetRotation = Quaternion.LookRotation(ball.position - transform.position, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}