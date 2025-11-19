using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI tmp;
    [SerializeField] private SceneTransition sceneTransition;

    private float timer;
    private TimeSpan time;

    bool sceneChanged;

    void Start()
    {
        
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
            sceneChanged = true;
            sceneTransition.SceneTransitionTo("kurt_scene");
        }
        
    }
}
