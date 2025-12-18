using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public Transform planet;
    public int numberOfMonsters = 3;
    public float spawnHeightOffset = 0.5f; // Small offset above surface to prevent clipping
    public float raycastDistance = 200f; // How far to raycast to find planet surface
    public float minDistanceBetweenMonsters = 30f;
    public Vector3 monsterRotationOffset = Vector3.zero; // Adjust if model is rotated weird
    
    void Start()
    {
        SpawnMonsters();
    }
    
    void SpawnMonsters()
    {
        Vector3[] spawnPositions = new Vector3[numberOfMonsters];
        
        for (int i = 0; i < numberOfMonsters; i++)
        {
            Vector3 surfacePosition = Vector3.zero;
            Vector3 surfaceNormal = Vector3.up;
            int attempts = 0;
            bool validPosition = false;
            
            do
            {
                // Pick a random direction from planet center
                Vector3 randomDirection = Random.onUnitSphere;
                
                // Start raycast from far above the planet surface, pointing toward center
                Vector3 rayStart = planet.position + randomDirection * raycastDistance;
                Vector3 rayDirection = -randomDirection; // Point toward planet center
                
                RaycastHit hit;
                if (Physics.Raycast(rayStart, rayDirection, out hit, raycastDistance * 2f))
                {
                    surfacePosition = hit.point;
                    surfaceNormal = hit.normal;
                    
                    // Check distance from other monsters
                    bool tooClose = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (Vector3.Distance(surfacePosition, spawnPositions[j]) < minDistanceBetweenMonsters)
                        {
                            tooClose = true;
                            break;
                        }
                    }
                    
                    if (!tooClose)
                    {
                        validPosition = true;
                    }
                }
                
                attempts++;
                if (attempts > 50)
                {
                    validPosition = true; // Give up and use last position
                }
                    
            } while (!validPosition);
            
            spawnPositions[i] = surfacePosition;
            
            // Calculate the rotation to align monster's up with surface normal
            Quaternion surfaceRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
            
            // Spawn slightly above the surface
            Vector3 spawnPosition = surfacePosition + surfaceNormal * spawnHeightOffset;
            
            // Instantiate monster with correct rotation
            GameObject monster = Instantiate(monsterPrefab, spawnPosition, surfaceRotation);
            
            // Apply additional rotation offset if model needs adjustment
            if (monsterRotationOffset != Vector3.zero)
            {
                monster.transform.Rotate(monsterRotationOffset, Space.Self);
            }
            
            // Freeze Rigidbody rotation so physics doesn't mess with orientation
            Rigidbody rb = monster.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.freezeRotation = true;
            }
        }
    }
}