using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerUI;
    [SerializeField] private SceneTransition sceneTransition;

    [SerializeField] public bool didWin = false;

    /***** EXAMPLE OF WHAT YOU PUT IN YOUR SCRIPT TO WIN OR LOSE: *****
     * 
     * [SerializeField] public LevelTemplate lvltmp;
     * bool didWin = true
     * lvltmp.FinishMinigame(didWin);
     * 
     */

    // ***** UPDATE THIS ON THE INSPECTOR TO CUSTOMIZE HOW LONG YOU WANT TO WAIT BEFORE TIMER STARTS AND HOW LONG AFTER TIMER ENDS TO WAIT

    [SerializeField] public float paddingtimerbefore = 2f;
    [SerializeField] public float paddingtimerafter = 2f;

    // *****

    private float timer = 6.7f;
    private TimeSpan time;

    bool sceneChanged;

    // Call To Finish Your Mini Game
    public void FinishMinigame(bool didWinParam)
    {
        if (timer <= 0)
        {
            return;
        }
        didWin = didWinParam;
        timer = 0;
        Debug.Log("Finished Game, didWin: " + didWin);
    }

    // Call To Dictate When The Player Can Start Playing The Mini Game
    public bool CanPlay()
    {
        if (paddingtimerbefore <= 0 || paddingtimerafter <= 0)
        {
            return true;
        }
        return false;
    }

    void Start()
    {
        timerUI.text = "06.700";
    }

    void Update()
    {
        // ***** Uncomment to debug if didWin worked
        //Debug.Log(didWin);

        if (paddingtimerbefore <= 0)
        {
            if (sceneChanged == false)
            {
                // Timer
                timer -= Time.deltaTime * 1.85f;
                time = TimeSpan.FromSeconds(timer);
                timerUI.text = time.ToString("ss'.'fff");
            }
            if (timer <= 0)
            {
                if (sceneChanged == false)
                {
                    timerUI.text = "00.000";
                    PersistentData.didWin = didWin;
                    sceneChanged = true;
                }             
                if (paddingtimerafter <= 0)
                {
                    sceneTransition.SceneTransitionTo("kurt_scene"); // main scene
                }
                paddingtimerafter -= Time.deltaTime;
            }
        }
        paddingtimerbefore -= Time.deltaTime;
    }
}
