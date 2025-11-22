using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaulSwitchManManager : MonoBehaviour
{
    [SerializeField] public bool LeftCard;
    [SerializeField] public bool RightCard;

    [SerializeField] LevelTemplate lvltmp;

    bool didWin;
    bool didFinish = false;

    void Update()
    {
        if (LeftCard == true && RightCard == true && didFinish == false)
        {
            didFinish = true;
            didWin = true;
            lvltmp.FinishMinigame(didWin);
        }
    }
}
