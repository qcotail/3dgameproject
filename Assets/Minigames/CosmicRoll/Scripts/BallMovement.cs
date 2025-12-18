using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;
    public Transform planet;
    
    [Header("Rolling Sound")]
    public AudioClip rollingSound;          // Assign your rock rolling sound
    public float minSpeedForSound = 2f;     // Minimum speed to play rolling sound
    public float maxVolume = 1f;            // Maximum volume at high speed
    public float volumeSpeedScale = 10f;    // Speed at which volume reaches max
    
    private Rigidbody rb;
    private Camera mainCamera;
    private AudioSource rollingAudioSource;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        
        // Create AudioSource for rolling sound
        rollingAudioSource = gameObject.AddComponent<AudioSource>();
        rollingAudioSource.clip = rollingSound;
        rollingAudioSource.loop = true;
        rollingAudioSource.playOnAwake = false;
        rollingAudioSource.volume = 0f;
        rollingAudioSource.spatialBlend = 0f; // 2D sound - always audible
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
    
    void Update()
    {
        UpdateRollingSound();
    }
    
    void UpdateRollingSound()
    {
        if (rollingSound == null) return;
        
        float currentSpeed = rb.velocity.magnitude;
        
        if (currentSpeed >= minSpeedForSound)
        {
            // Start playing if not already
            if (!rollingAudioSource.isPlaying)
            {
                rollingAudioSource.Play();
            }
            
            // Scale volume based on speed
            float volumePercent = Mathf.Clamp01((currentSpeed - minSpeedForSound) / volumeSpeedScale);
            rollingAudioSource.volume = volumePercent * maxVolume;
            
            // Optional: scale pitch slightly based on speed for variety
            rollingAudioSource.pitch = 0.8f + (volumePercent * 0.4f);
        }
        else
        {
            // Fade out when slow
            rollingAudioSource.volume = Mathf.Lerp(rollingAudioSource.volume, 0f, 5f * Time.deltaTime);
            
            if (rollingAudioSource.volume < 0.01f && rollingAudioSource.isPlaying)
            {
                rollingAudioSource.Stop();
            }
        }
    }
}