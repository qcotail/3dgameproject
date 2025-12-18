using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Death Animation")]
    public float fadeSpeed = 2f;
    public float shrinkSpeed = 1f;
    
    [Header("Star Burst")]
    public GameObject starPrefab;           // Assign a star/crystal prefab
    public Transform player;                // Auto-finds if not set
    public Transform planet;                // For burst direction
    public int starCount = 3;               // Stars spawned on death
    public float starSpawnRadius = 0.5f;    // Spread of spawned stars
    
    [Header("Sound Effects")]
    public AudioClip deathSound;            // Sound when monster is hit
    [Range(0f, 1f)] public float deathVolume = 1f;
    
    private Renderer[] renderers;
    private bool isDying = false;
    private Vector3 originalScale;
    
    void Start()
    {
        originalScale = transform.localScale;
        
        // Auto-find player by name if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Ball");
            if (playerObj != null) player = playerObj.transform;
        }
        
        // Auto-find planet by name if not assigned
        if (planet == null)
        {
            GameObject planetObj = GameObject.Find("YoshiBoxBreakPlanet");
            if (planetObj != null) planet = planetObj.transform;
        }
        
        // Get all renderers in case monster has multiple parts
        renderers = GetComponentsInChildren<Renderer>();
        
        // Make sure materials can fade
        foreach (Renderer rend in renderers)
        {
            // Change to transparent mode
            rend.material.SetFloat("_Mode", 3);
            rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            rend.material.SetInt("_ZWrite", 0);
            rend.material.DisableKeyword("_ALPHATEST_ON");
            rend.material.EnableKeyword("_ALPHABLEND_ON");
            rend.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            rend.material.renderQueue = 3000;
        }
        
        Debug.Log($"Monster initialized with {renderers.Length} renderers");
    }
    
    void Update()
    {
        if (isDying)
        {
            // Fade out
            foreach (Renderer rend in renderers)
            {
                Color color = rend.material.color;
                color.a -= fadeSpeed * Time.deltaTime;
                rend.material.color = color;
            }
            
            // Shrink (use Lerp with shrinkSpeed as rate - higher = faster)
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, shrinkSpeed * Time.deltaTime);
            
            // Destroy when shrunk to 10% of original size or fully transparent
            float scaleRatio = transform.localScale.magnitude / originalScale.magnitude;
            bool isShrunk = scaleRatio < 0.1f;
            bool isFaded = renderers.Length > 0 && renderers[0].material.color.a <= 0.1f;
            
            if (isShrunk || isFaded)
            {
                Destroy(gameObject);
            }
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Monster COLLISION with: {collision.gameObject.name}");
        CheckAndDie(collision.gameObject);
    }
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Monster TRIGGER with: {other.gameObject.name}");
        CheckAndDie(other.gameObject);
    }
    
    void CheckAndDie(GameObject other)
    {
        // Check by name instead of tag
        if (other.name == "Ball" && !isDying)
        {
            Debug.Log("Monster dying!");
            Die();
        }
    }
    
    void Die()
    {
        isDying = true;
        
        // Play death sound (2D, always audible)
        if (deathSound != null)
        {
            PlaySound2D(deathSound, deathVolume);
        }
        
        // Disable ALL colliders so ball passes through
        Collider[] cols = GetComponentsInChildren<Collider>();
        foreach (Collider col in cols)
        {
            col.enabled = false;
        }
        
        // Spawn stars!
        SpawnStars();
    }
    
    // Play sound as 2D (no distance falloff, always audible)
    void PlaySound2D(AudioClip clip, float volume)
    {
        // Create a standalone audio object that won't be affected by monster destruction
        GameObject tempAudio = new GameObject("TempAudio_" + clip.name);
        tempAudio.hideFlags = HideFlags.HideInHierarchy; // Hide from hierarchy to reduce clutter
        AudioSource audioSource = tempAudio.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 0f; // 0 = 2D, 1 = 3D
        audioSource.Play();
        Object.Destroy(tempAudio, clip.length + 0.5f);
    }
    
    void SpawnStars()
    {
        if (starPrefab == null || player == null) return;
        
        // Calculate burst direction (away from planet, or just up if no planet)
        Vector3 burstDirection = Vector3.up;
        if (planet != null)
        {
            burstDirection = (transform.position - planet.position).normalized;
        }
        
        for (int i = 0; i < starCount; i++)
        {
            // Spawn at slightly random positions around the monster
            Vector3 spawnOffset = Random.insideUnitSphere * starSpawnRadius;
            Vector3 spawnPos = transform.position + spawnOffset + burstDirection * 0.5f;
            
            GameObject star = Instantiate(starPrefab, spawnPos, Random.rotation);
            
            // Initialize the star pickup behavior
            StarPickup pickup = star.GetComponent<StarPickup>();
            if (pickup != null)
            {
                // Each star bursts in a slightly different direction
                Vector3 individualBurst = burstDirection + Random.insideUnitSphere * 0.5f;
                pickup.Initialize(player, individualBurst.normalized);
            }
        }
    }
}