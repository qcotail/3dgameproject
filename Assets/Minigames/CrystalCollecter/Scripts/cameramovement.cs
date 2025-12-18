using UnityEngine;

public class SimpleBallCamera : MonoBehaviour
{
    public Transform ball;
    public Transform planet;
    public Rigidbody ballRb;
    public float distance = 30f;
    public float height = 75f;
    public float sideOffset = 5f;
    public float smoothSpeed = 5f;
    public float rotationSpeed = 5f;
    
    [Header("Dead Zone Settings")]
    public float minSpeedToFollow = 50f;
    public float viewportMargin = 0.2f; // Ball must be within 20% of screen edges to be "in view"
    public float snapBackSpeed = 0.1f;
    public float minRotationSpeed = 0.1f;
    public float decelerationRate = 0.995f;
    
    private Vector3 currentVelocity;
    private Vector3 cameraVelocity;
    private bool isReturning = false;
    private Camera cam;
    
    void Start()
    {
        cam = Camera.main;
    }
    
    void LateUpdate()
    {
        if (ball == null || ballRb == null || planet == null || cam == null) return;
        
        float ballSpeed = ballRb.velocity.magnitude;
        
        // Check if ball is in camera viewport
        Vector3 viewportPoint = cam.WorldToViewportPoint(ball.position);
        bool ballInView = viewportPoint.x > viewportMargin && viewportPoint.x < (1f - viewportMargin) &&
                          viewportPoint.y > viewportMargin && viewportPoint.y < (1f - viewportMargin) &&
                          viewportPoint.z > 0; // z > 0 means in front of camera
        
        // Calculate planet up
        Vector3 planetUp = (ball.position - planet.position).normalized;
        
        // Get ball's velocity projected onto planet surface
        Vector3 velocity = ballRb.velocity;
        Vector3 moveDirection = Vector3.ProjectOnPlane(velocity, planetUp).normalized;
        
        // Hysteresis logic: start returning when ball leaves view, stop when back in view
        if (!ballInView)
        {
            isReturning = true;
        }
        else if (ballInView && viewportPoint.x > (viewportMargin + 0.1f) && viewportPoint.x < (0.9f - viewportMargin) &&
                 viewportPoint.y > (viewportMargin + 0.1f) && viewportPoint.y < (0.9f - viewportMargin))
        {
            // Only stop returning when ball is comfortably in center (extra 10% margin for hysteresis)
            isReturning = false;
        }
        
        // Check if ball is moving fast enough
        bool shouldFollow = ballSpeed > minSpeedToFollow;
        
        if (shouldFollow && moveDirection.magnitude > 0.1f)
        {
            // Normal following behavior
            Vector3 targetPosition = ball.position - moveDirection * distance + planetUp * height + Vector3.Cross(planetUp, moveDirection) * sideOffset;
            Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / smoothSpeed);
            
            // Cap camera velocity at 1.2x ball speed
            Vector3 positionDelta = newPosition - transform.position;
            cameraVelocity = Vector3.ClampMagnitude(positionDelta / Time.deltaTime, ballSpeed * 1.2f);
            
            transform.position = newPosition;
            
            // Look at ball with normal rotation speed
            Vector3 lookDirection = ball.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (isReturning)
        {
            // Ball out of view - move back slowly
            Vector3 offset = transform.position - ball.position;
            Vector3 offsetProjected = Vector3.ProjectOnPlane(offset, planetUp).normalized;
            Vector3 targetPosition = ball.position + offsetProjected * distance + planetUp * height;
            
            // Give initial velocity if camera is stopped
            if (cameraVelocity.magnitude < 1f)
            {
                Vector3 directionToBall = (targetPosition - transform.position).normalized;
                cameraVelocity = directionToBall * 5f;
            }
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, 1f / snapBackSpeed);
            
            // Gradually kill momentum when snapping back
            cameraVelocity *= decelerationRate;
            
            // Rotate to look at ball
            Quaternion targetRotation = Quaternion.LookRotation(ball.position - transform.position, planetUp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minRotationSpeed * Time.deltaTime);
        }
        else
        {
            // Ball moving slow and in range - coast with momentum
            transform.position += cameraVelocity * Time.deltaTime;
            cameraVelocity *= decelerationRate;
            
            // Always look at ball
            Vector3 lookDirection = ball.position - transform.position;
            if (lookDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection, planetUp);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, minRotationSpeed * Time.deltaTime);
            }
        }
    }
    
    void OnGUI()
    {
        if (ball == null || ballRb == null || cam == null) return;
        
        float ballSpeed = ballRb.velocity.magnitude;
        Vector3 viewportPoint = cam.WorldToViewportPoint(ball.position);
        float camSpeed = cameraVelocity.magnitude;
        
        bool ballInView = viewportPoint.x > viewportMargin && viewportPoint.x < (1f - viewportMargin) &&
                          viewportPoint.y > viewportMargin && viewportPoint.y < (1f - viewportMargin) &&
                          viewportPoint.z > 0;
        
        // Create a box in top-left corner
        GUI.Box(new Rect(10, 10, 250, 160), "Camera Debug Info");
        
        // Display values
        GUI.Label(new Rect(20, 35, 230, 20), $"Ball Speed: {ballSpeed:F2}");
        GUI.Label(new Rect(20, 55, 230, 20), $"Ball In View: {ballInView}");
        GUI.Label(new Rect(20, 75, 230, 20), $"Viewport: ({viewportPoint.x:F2}, {viewportPoint.y:F2})");
        GUI.Label(new Rect(20, 95, 230, 20), $"Camera Velocity: {camSpeed:F2}");
        GUI.Label(new Rect(20, 115, 230, 20), $"Is Returning: {isReturning}");
        GUI.Label(new Rect(20, 135, 230, 20), $"Min Speed Threshold: {minSpeedToFollow}");
        GUI.Label(new Rect(20, 155, 230, 20), $"Viewport Margin: {viewportMargin:F2}");
    }
}