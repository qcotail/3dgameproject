using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PersistentDataTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myTextMeshProUGUI;

    // Inputs
    bool upInput;
    bool downInput;

    void Start()
    {
        myTextMeshProUGUI.text = PersistentData.lives.ToString();
    }

    void Update()
    {
        upInput = Input.GetKeyDown(KeyCode.UpArrow);
        if (upInput)
        {
            PersistentData.lives += 1;
        }

        downInput = Input.GetKeyDown(KeyCode.DownArrow);
        if (downInput)
        {
            PersistentData.lives -= 1;
        }
        myTextMeshProUGUI.text = PersistentData.lives.ToString();
    }
}
