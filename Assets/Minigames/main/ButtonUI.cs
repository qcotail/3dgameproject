using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonUI : MonoBehaviour
{
    //[SerializeField] private SceneTransition sceneTransition;
    public void NewGameButton()
    {
        // Reset game data before starting new game
        PersistentData.ResetGame();
        SceneManager.LoadScene("othermain");
    }
}
