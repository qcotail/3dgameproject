using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicRollManager : MonoBehaviour
{
    [SerializeField] LevelTemplate lvltmp;
    
    private bool didFinish = false;

    void Update()
    {
        // Check if timer ran out and game hasn't finished yet
        if (lvltmp != null && lvltmp.timer <= 0 && !didFinish)
        {
            didFinish = true;
            lvltmp.FinishMinigame(false); // Player loses if timer runs out
            Debug.Log("CosmicRoll: Timer ran out - Player lost");
        }
    }
}

