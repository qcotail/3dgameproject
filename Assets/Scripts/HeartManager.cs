using UnityEngine;
using UnityEngine.SceneManagement;

public class HeartManager : MonoBehaviour
{
    [Header("Heart GameObjects")]
    [SerializeField] private GameObject heart1;
    [SerializeField] private GameObject heart2;
    [SerializeField] private GameObject heart3;
    
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverText; // Text or Sprite GameObject to show on game over
    [SerializeField] private bool useGameOverScene = false; // Set to true if you want to use separate scene
    [SerializeField] private string gameOverSceneName = "Gameover";
    [SerializeField] private SceneTransition sceneTransition;
    
    private GameObject[] hearts;
    
    void Start()
    {
        // Auto-find hearts if not assigned
        if (heart1 == null) heart1 = GameObject.Find("heart");
        if (heart2 == null) heart2 = GameObject.Find("heart (1)");
        if (heart3 == null) heart3 = GameObject.Find("heart (2)");
        
        // Auto-find SceneTransition if not assigned
        if (sceneTransition == null)
        {
            sceneTransition = FindObjectOfType<SceneTransition>();
        }
        
        // Auto-find game over text if not assigned
        if (gameOverText == null)
        {
            gameOverText = GameObject.Find("GameOverText");
        }
        
        // Hide game over text initially
        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
        
        hearts = new GameObject[] { heart1, heart2, heart3 };
        
        // Update hearts display
        UpdateHearts();
        
        // Check for game over
        CheckGameOver();
    }
    
    public void UpdateHearts()
    {
        int lives = Mathf.RoundToInt(PersistentData.lives);
        
        // Show/hide hearts based on lives
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                // Heart is visible if i < lives (0-based index)
                hearts[i].SetActive(i < lives);
            }
        }
        
        Debug.Log($"Lives: {lives}, Hearts visible: {lives}");
    }
    
    void CheckGameOver()
    {
        if (PersistentData.lives <= 0)
        {
            Debug.Log("Game Over! No lives remaining.");
            ShowGameOver();
        }
    }
    
    public void ShowGameOver()
    {
        // Show game over text/sprite
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
            Debug.Log("Game Over text/sprite displayed");
        }
        else
        {
            Debug.LogWarning("Game Over text/sprite not assigned! Assign it in the inspector or name it 'GameOverText'");
        }
        
        // If using separate game over scene, transition to it
        if (useGameOverScene)
        {
            StartCoroutine(GameOverSequence());
        }
    }
    
    System.Collections.IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2f); // Brief pause before game over
        
        if (sceneTransition != null)
        {
            sceneTransition.SceneTransitionTo(gameOverSceneName);
        }
        else
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }
}

