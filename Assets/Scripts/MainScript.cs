using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScript : MonoBehaviour
{
    [SerializeField] public string[] MinigameScenes;
    [SerializeField] SceneTransition sceneTransition;
    public Sprite[] loadedSprites;

    void Start()
    {
        StartCoroutine(LoadMain());
    }
    IEnumerator LoadMain()
    {
        if (PersistentData.didWin == false)
        {
            LostLife();
            PlayAnimation();
            yield return new WaitForSeconds(2); // whatever how long the animation is
        }
        yield return new WaitForSeconds(2); // idk some time before switching to the next minigame for whatever reason
        NextMiniGame();
    }
    // Will either play an animation from losing a life or gaining a life
    void PlayAnimation()
    {
        Debug.Log("Animation Plays");
    }
    void LostLife()
    {
        PersistentData.lives -= 1f;
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
