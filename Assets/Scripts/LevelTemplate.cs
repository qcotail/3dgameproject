using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI timerUI;
    [SerializeField] private SceneTransition sceneTransition;

    // ***** UPDATE THIS TO FALSE OR TRUE IF PLAYER WON OR NOT, BY DEFAULT IT IS FALSE ***** \\
    [SerializeField] public bool didWin = false;

    // ***** EXAMPLE OF WHAT YOU PUT IN YOUR SCRIPT TO WIN OR LOSE: ***** \\
    //[SerializeField] public LevelTemplate lvltmp;
    //lvltmp.didWin = true;
    // *****                                                                           ***** \\

    // ***** UPDATE THIS TO CUSTOMIZE HOW LONG YOU WANT TO WAIT BEFORE TIMER STARTS AND HOW LONG AFTER TIMER ENDS TO WAIT ***** \\
    [SerializeField] public float paddingtimerbefore = 2f;
    [SerializeField] public float paddingtimerafter = 2f;
    // *****                                                                           ***** \\

    private float timer = 6.7f;
    private TimeSpan time;

    bool sceneChanged;

    void Start()
    {
        timerUI.text = "06.700";
    }

    void Update()
    {
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
                    sceneTransition.SceneTransitionTo("kurt_scene");
                }
                paddingtimerafter -= Time.deltaTime;
            }
        }
        paddingtimerbefore -= Time.deltaTime;
    }
}
