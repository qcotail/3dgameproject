using UnityEngine;

public class StarPickup : MonoBehaviour
{
    public float burstForce = 5f;           // Initial burst outward
    public float homeDelay = 0.3f;          // Time before homing starts
    public float homeSpeed = 15f;           // Speed when homing toward player
    public float homeAcceleration = 20f;    // How fast it speeds up
    public float collectDistance = 1f;      // Distance to collect
    public float spinSpeed = 360f;          // Spin speed in degrees/sec
    public float lingerTimeAfterCollect = 5f; // How long star stays after being collected
    
    [Header("Sound")]
    public AudioClip sparkleSound;          // Looping sparkle sound
    [Range(0f, 1f)] public float sparkleVolume = 0.5f;
    
    private Transform player;
    private Vector3 velocity;
    private float timer = 0f;
    private bool isHoming = false;
    private bool isCollected = false;
    private float currentSpeed;
    private AudioSource audioSource;
    
    public void Initialize(Transform playerTarget, Vector3 burstDirection)
    {
        player = playerTarget;
        
        // Add some randomness to burst direction
        Vector3 randomOffset = Random.insideUnitSphere * 0.5f;
        velocity = (burstDirection + randomOffset).normalized * burstForce;
        
        currentSpeed = 0f;
    }
    
    void Update()
    {
        // Keep spinning even after collected
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.right, spinSpeed * 0.7f * Time.deltaTime, Space.Self);
        
        // If already collected, just spin and wait to be destroyed
        if (isCollected) return;
        
        timer += Time.deltaTime;
        
        if (!isHoming && timer >= homeDelay)
        {
            isHoming = true;
        }
        
        if (isHoming && player != null)
        {
            // Accelerate toward player
            currentSpeed = Mathf.MoveTowards(currentSpeed, homeSpeed, homeAcceleration * Time.deltaTime);
            
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            velocity = directionToPlayer * currentSpeed;
            
            // Check if close enough to collect
            if (Vector3.Distance(transform.position, player.position) < collectDistance)
            {
                Collect();
                return;
            }
        }
        else
        {
            // During burst phase, slow down
            velocity = Vector3.Lerp(velocity, Vector3.zero, 3f * Time.deltaTime);
        }
        
        // Move
        transform.position += velocity * Time.deltaTime;
    }
    
    void Collect()
    {
        if (isCollected) return;
        isCollected = true;
        
        // You can add score/points here, play a sound, etc.
        // Example: GameManager.Instance.AddScore(10);
        Debug.Log("Star collected!");
        
        // Stop moving
        velocity = Vector3.zero;
        
        // Destroy after linger time
        Destroy(gameObject, lingerTimeAfterCollect);
    }
    
    void Start()
    {
        // Auto-destroy if it somehow doesn't get collected
        Destroy(gameObject, 40f);
        
        // Play sparkle sound (loops while star exists)
        if (sparkleSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sparkleSound;
            audioSource.volume = sparkleVolume;
            audioSource.spatialBlend = 0f; // 2D sound
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}

