using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkWinManager : MonoBehaviour
{
    [SerializeField] LevelTemplate lvltmp;
    public GameObject homelanderVid;
    public GameObject lossVid;
    //public float timer = 6.7f;
    public bool alreadyOver = false;

    private bool didFinish = false;

    private void Start()
    {
        homelanderVid.SetActive(false);
        lossVid.SetActive(false);
    }

    private void Update()
    {
        if (lvltmp.timer < 0 && !alreadyOver)
        {
            alreadyOver = true;
            didFinish = false;
            lvltmp.FinishMinigame(false);
            lossVid.SetActive(true);
        }
        //timer -= Time.deltaTime;

        //if (timer < 0)
        //{
        //    didFinish = false;
        //    lossVid.SetActive(true);
        //}
    }

    public void OnMilkDelivered()
    {
        if (didFinish)
            return;

        didFinish = true;
        alreadyOver = true;

        lvltmp.FinishMinigame(true);
        homelanderVid.SetActive(true);
    }
}
