using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaulSwitchManManager : MonoBehaviour
{
    [SerializeField] public bool LeftCard;
    [SerializeField] public bool RightCard;

    [SerializeField] LevelTemplate lvltmp;

    public GameObject saullossvideo;
    public GameObject saulwinvideo;

    public Canvas canvas;

    bool didWin;
    bool didFinish = false;

    private void Start()
    {
        saullossvideo.SetActive(false);
        saulwinvideo.SetActive(false);
    }
    void Update()
    {
        if (lvltmp.timer <= 0 && didFinish == false)
        {
            lvltmp.FinishMinigame(false);
            canvas.enabled = false;
            saullossvideo.SetActive(true);
            didFinish = true;
        }
        if (LeftCard == true && RightCard == true && didFinish == false)
        {
            didWin = true;
            lvltmp.FinishMinigame(didWin);
            canvas.enabled = false;
            saulwinvideo.SetActive(true);
            didFinish = true;
        }
    }
}
