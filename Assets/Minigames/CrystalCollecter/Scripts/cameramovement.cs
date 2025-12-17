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
    
    [Header("Dead Zone Settings")]
    public float minSpeedToFollow = 5f;
    public float maxDistanceFromBall = 40f;
    public float snapBackSpeed = 2f;
    public float minRotationSpeed = 0.5f;
    public float decelerationRate = 0.95f; // NEW: How fast camera slows down (0.9-0.99, higher = slower deceleration)
    
    private Vector3 currentVelocity;
    private Vector3 cameraVelocity; // NEW: Track camera's own momentum
    
    void LateUpdate()
    {
        if (ball == null || ballRb == null || planet == null) return;
        
        float ballSpeed = ballRb.velocity.magnitude;
        float distanceToBall = Vector3.Distance(transform.position, ball.position);
        
        // Calculate planet up
        Vector3 planetUp = (ball.position - planet.position).normalized;
        
        // Get ball's velocity projected onto planet surface
        Vector3 velocity = ballRb.velocity;
        Vector3 moveDirection = Vector3.ProjectOnPlane(velocity, planetUp).normalized;
        
        // Check if ball is moving fast enough OR too far away
        bool shouldFollow = ballSpeed > minSpeedToFollow || distanceToBall > maxDistanceFromBall;
        
        if (shouldFollow && moveDirection.magnitude > 0.1f)
        {
            // Normal following behavior
            Vector3 targetPosition = ball.position - moveDirection * distance + planetUp * height + Vector3.Cross(planetUp, moveDirection) * sideOffset;
            Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / smoothSpeed);
            
            // Track camera velocity for momentum
            cameraVelocity = (newPosition - transform.position) / Time.deltaTime;
            transform.position = newPosition;
            
            // Look at ball with normal rotation speed
            Vector3 lookDirection = ball.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (distanceToBall > maxDistanceFromBall)
        {
            // Ball too far - move back slowly
            Vector3 offset = transform.position - ball.position;
            Vector3 offsetProjected = Vector3.ProjectOnPlane(offset, planetUp).normalized;
            Vector3 targetPosition = ball.position + offsetProjected * distance + planetUp * height;
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / snapBackSpeed);
            
            // Gradually kill momentum when snapping back
            cameraVelocity *= decelerationRate;
            
            // Rotate slowly
            Quaternion targetRotation = Quaternion.LookRotation(ball.position - transform.position, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minRotationSpeed * Time.deltaTime);
        }
        else
        {
            // Ball moving slow and in range - coast with momentum
            // Apply remaining velocity and decelerate
            transform.position += cameraVelocity * Time.deltaTime;
            cameraVelocity *= decelerationRate; // Gradually slow down
            
            // Still look at ball, but don't rotate if camera has stopped
            if (cameraVelocity.magnitude > 0.1f)
            {
                Vector3 lookDirection = ball.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, planetUp);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minRotationSpeed * Time.deltaTime);
            }
        }
    }
}