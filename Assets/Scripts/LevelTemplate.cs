using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI tmp;
    [SerializeField] private SceneTransition sceneTransition;

    // ***** UPDATE THIS TO FALSE OR TRUE IF PLAYER WON OR NOT, BY DEFAULT IT IS FALSE ***** \\
    public bool didWin;
    // *****                                                                           ***** \\

    private float timer;
    private TimeSpan time;

    bool sceneChanged;

    void Start()
    {
        didWin = false;
    }

    void Update()
    {
        if (sceneChanged)
        {
            return;
        }
        // Timer
        timer += Time.deltaTime * 1.85f;
        time = TimeSpan.FromSeconds(timer);
        tmp.text = time.ToString("ss'.'fff");
        if (timer >= 6.7f)
        {
            PersistentData.didWin = didWin;
            sceneChanged = true;
            sceneTransition.SceneTransitionTo("kurt_scene");
        }
        
    }
}
