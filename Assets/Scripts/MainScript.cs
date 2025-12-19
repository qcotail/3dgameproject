using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    [SerializeField] public string[] MinigameScenes;
    [SerializeField] SceneTransition sceneTransition;
    public Sprite[] hearts;

    void Start()
    {
        StartCoroutine(LoadMain());
    }
    IEnumerator LoadMain()
    {
        Debug.Log($"LoadMain started - Current lives: {PersistentData.lives}, didWin: {PersistentData.didWin}, currlevels: {PersistentData.currlevels}");
        
        // Check for game over first (before processing win/loss)
        // Game should end when lives reach 0, which happens after 3 losses (3 → 2 → 1 → 0)
        if (PersistentData.lives <= 0)
        {
            Debug.Log("Game Over - No lives remaining at start");
            yield return StartCoroutine(TransitionToGameOver());
            yield break;
        }
        
        // Only process win/loss if we've actually played at least one minigame
        // This prevents the first entry to othermain (before any minigame) from counting as a loss
        if (PersistentData.currlevels > 0)
        {
            // Only lose a life if the player lost the minigame
            // This should only happen when didWin is false (player lost)
            if (PersistentData.didWin == false)
            {
                Debug.Log($"Player lost minigame. Lives before: {PersistentData.lives}");
                LostLife();
                Debug.Log($"Lives after losing: {PersistentData.lives}");
                PlayAnimation();
                yield return new WaitForSeconds(2); // whatever how long the animation is
                
                // Check again after losing a life - game over should trigger when lives reach 0
                // This means after the 3rd loss (when lives go from 1 to 0)
                if (PersistentData.lives <= 0)
                {
                    Debug.Log("Game Over - No lives remaining after loss (3rd loss)");
                    yield return StartCoroutine(TransitionToGameOver());
                    yield break;
                }
            }
            else
            {
                Debug.Log("Player won minigame - no life lost");
            }
        }
        else
        {
            Debug.Log("First time entering othermain - skipping win/loss processing");
        }
        
        // Reset didWin for next minigame to prevent stale values
        PersistentData.didWin = false;
        
        yield return new WaitForSeconds(2); // idk some time before switching to the next minigame for whatever reason
        NextMiniGame();
    }
    
    IEnumerator TransitionToGameOver()
    {
        yield return new WaitForSeconds(2f); // Brief pause before game over
        
        // Show game over UI in current scene instead of transitioning
        HeartManager heartManager = FindObjectOfType<HeartManager>();
        if (heartManager != null)
        {
            heartManager.ShowGameOver();
        }
        else
        {
            Debug.LogWarning("HeartManager not found! Cannot show game over UI.");
        }
    }
    // Will either play an animation from losing a life or gaining a life
    void PlayAnimation()
    {
        Debug.Log("Animation Plays");
    }
    void LostLife()
    {
        if (PersistentData.lives > 0)
        {
            PersistentData.lives -= 1f;
            Debug.Log($"LostLife called - Lives now: {PersistentData.lives}");
            
            // Update hearts display after losing a life
            HeartManager heartManager = FindObjectOfType<HeartManager>();
            if (heartManager != null)
            {
                heartManager.UpdateHearts();
            }
        }
        else
        {
            Debug.LogWarning("LostLife called but lives already at 0!");
        }
    }

    // logic for picking minigame scene
    void NextMiniGame()
    {
        // Advance level counter each time we move to a new minigame
        PersistentData.currlevels += 1;

        // 10th minigame: always go to CatSkater (as before)
        if (PersistentData.currlevels == 10)
        {
            sceneTransition.SceneTransitionTo("CatSkater");
        }
        // 5th minigame: always go to CosmicRoll
        else if (PersistentData.currlevels == 5)
        {
            sceneTransition.SceneTransitionTo("CosmicRoll");
        }
        // First 4 minigames: non-repeating shuffled order of the 4 specified games
        else if (PersistentData.currlevels <= 4)
        {
            // Initialize and shuffle the order once per run
            if (!PersistentData.minigameOrderInitialized || PersistentData.minigameOrder == null || PersistentData.minigameOrder.Length != 4)
            {
                string[] baseOrder = new string[] { "Huntsman", "LosPollosHermanos", "SaulSwitchNumbers", "Yummers" };

                // Fisher-Yates shuffle
                for (int i = 0; i < baseOrder.Length; i++)
                {
                    int j = UnityEngine.Random.Range(i, baseOrder.Length);
                    string tmp = baseOrder[i];
                    baseOrder[i] = baseOrder[j];
                    baseOrder[j] = tmp;
                }

                PersistentData.minigameOrder = baseOrder;
                PersistentData.minigameOrderInitialized = true;
            }

            int index = (int)PersistentData.currlevels - 1; // 0–3
            string nextScene = PersistentData.minigameOrder[index];
            sceneTransition.SceneTransitionTo(nextScene);
        }
        else
        {
            // After the first 5, keep using the original random selection behavior
            int randomnum = UnityEngine.Random.Range(0, MinigameScenes.Length);
            sceneTransition.SceneTransitionTo(MinigameScenes[randomnum]);
        }

        Debug.Log("Minigame Loaded");
    }
}

