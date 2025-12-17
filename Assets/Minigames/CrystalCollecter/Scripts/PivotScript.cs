using UnityEngine;

public class PlanetCameraTarget : MonoBehaviour
{
    public Transform ball;
    public Transform planet;
    
    void LateUpdate()
    {
        if (ball == null || planet == null) return;
        
        // Follow ball's position
        transform.position = ball.position;
        
        // Orient "up" away from planet
        Vector3 up = (ball.position - planet.position).normalized;
        transform.up = up;
    }
}