using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkWinManager : MonoBehaviour
{
    [SerializeField] LevelTemplate lvltmp;
    public GameObject homelanderVid;
    public GameObject lossVid;
    public float timer = 6.7f;

    private bool didFinish = false;

    private void Start()
    {
        homelanderVid.SetActive(false);
        lossVid.SetActive(false);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            didFinish = false;
            lossVid.SetActive(true);
        }
    }

    public void OnMilkDelivered()
    {
        if (didFinish)
            return;

        didFinish = true;

        lvltmp.FinishMinigame(true);
        homelanderVid.SetActive(true);
    }
}
