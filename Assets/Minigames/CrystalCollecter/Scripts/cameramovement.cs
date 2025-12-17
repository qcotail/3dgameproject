using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform ball;
    public Transform planet;
    public float distance = 10f;
    public float height = 5f;
    public float rotationSpeed = 5f;
    
    void LateUpdate()
    {
        if (ball == null || planet == null) return;
        
        // Direction from planet to ball (ball's "up")
        Vector3 up = (ball.position - planet.position).normalized;
        
        // Position camera behind and above ball
        Vector3 targetPosition = ball.position - ball.forward * distance + up * height;
        
        // Smooth position
        transform.position = Vector3.Lerp(transform.position, targetPosition, 10f * Time.deltaTime);
        
        // Smooth rotation to look at ball
        Quaternion targetRotation = Quaternion.LookRotation(ball.position - transform.position, up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}