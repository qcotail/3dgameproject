using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Animator transitionAnim;

    //Scene PreviousScene;

    // CrossFade Variables For Animations
    private string currentState = "Default";
    private bool fadeIn = false;
    private bool fadeOut = false;

    // The Instance Of The SceneTransition Object
    //public SceneTransition instance;

    // Make Sure Scene Transition Game Object Doesn't Get Destroyed
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    // Testing
    //void Start()
    //{
        //StartCoroutine(Testing());
        //SceneTransitionTo("LevelTemplateTest");
    //}
    //IEnumerator Testing()
    //{
    //    SceneTransitionTo("kurt_scene");
    //    yield return new WaitForSeconds(5);
    //    SceneTransitionTo("kurt_scene_2");
    //}

    void Update()
    {
        // CrossFade Update
        var state = GetState();
        if (state.Equals(currentState)) return;
        currentState = state;
        transitionAnim.CrossFade(currentState, 1f, 0);
    }

    // Call To Scene Transition To A Specific Scene
    public void SceneTransitionTo(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }
    IEnumerator LoadLevel(string sceneName)
    {
        fadeOut = true;
        yield return new WaitForSeconds(1);
        fadeIn = true;
        //SceneManager.LoadSceneAsync(sceneName);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        yield return new WaitForSeconds(1);

        fadeIn = false;
        fadeOut = false;
    }

    // For CrossFade
    private string GetState()
    {
        if (fadeIn) return "FadeIn";
        if (fadeOut) return "FadeOut";
        return "Default";
    }
}
