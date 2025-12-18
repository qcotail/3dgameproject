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
        if (PersistentData.currlevels >= 10) {
            sceneTransition.SceneTransitionTo("CatSkater");
        } else
        {
            int randomnum = UnityEngine.Random.Range(0, MinigameScenes.Length);
            sceneTransition.SceneTransitionTo(MinigameScenes[randomnum]);
        }
            Debug.Log("Minigame Loaded");
    }
}
