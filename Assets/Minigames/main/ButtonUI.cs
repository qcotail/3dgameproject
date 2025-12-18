using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    //[SerializeField] private SceneTransition sceneTransition;
    public void NewGameButton()
    {
        SceneManager.LoadScene("othermain");
    }
}
