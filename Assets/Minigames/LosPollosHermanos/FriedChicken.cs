using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FriedChicken : MonoBehaviour {

    public bool[] alreadyClicked = new bool[6];
    public int[] chickenReady = new int[4];
    public GameObject[] correspondingObject = new GameObject[6];
    public bool[] objectsMoving = new bool[6];
    public float[] movementTimer = new float[6];
    public float cookTimer = 3f;
    public bool cooked = true;
    [SerializeField] LevelTemplate lvltmp;
    public bool[] chickenPulled = new bool[4];
    public int i = 0;
    public bool alreadyOver = false;
    public GameObject rawChicken;
    public GameObject friedChicken;
    public GameObject[] correspondingCheckmark = new GameObject[6];
    public GameObject gusVideo;
    public void buttonClicked(int buttonNumber) {
        objectsMoving[buttonNumber - 1] = true;
        if (buttonNumber == chickenReady[0] || buttonNumber == chickenReady[1] || buttonNumber == chickenReady[2] || buttonNumber == chickenReady[3]) {
            if (!alreadyClicked[buttonNumber - 1]) {
                alreadyClicked[buttonNumber - 1] = true;
                chickenPulled[i] = true;
                i += 1;
                correspondingCheckmark[buttonNumber - 1].SetActive(false);
                GameObject goodChicken1 = Instantiate(friedChicken, correspondingObject[buttonNumber - 1].transform.position, Random.rotation);
                goodChicken1.AddComponent<Rigidbody>();
                goodChicken1.AddComponent<BoxCollider>();
                BoxCollider newBox1 = goodChicken1.GetComponent<BoxCollider>();
                newBox1.size = new Vector3(0.001f, 0.001f, 0.001f);
                goodChicken1.transform.position += new Vector3(0, 0.8f, 0);
                GameObject goodChicken2 = Instantiate(friedChicken, correspondingObject[buttonNumber - 1].transform.position, Random.rotation);
                goodChicken2.AddComponent<Rigidbody>();
                goodChicken2.AddComponent<BoxCollider>();
                BoxCollider newBox2 = goodChicken2.GetComponent<BoxCollider>();
                newBox2.size = new Vector3(0.001f, 0.001f, 0.001f);
                goodChicken2.transform.position += new Vector3(0, 0.8f, 0.7f);
                //LosPollosSounds.play("");
            }
        }
        else {
            if (!alreadyOver) {
                alreadyOver = true;
                //LosPollosSounds.play("");
                StartCoroutine(endMinigame());
                lvltmp.FinishMinigame(false);
                correspondingCheckmark[0].SetActive(false);
                correspondingCheckmark[1].SetActive(false);
                correspondingCheckmark[2].SetActive(false);
                correspondingCheckmark[3].SetActive(false);
                correspondingCheckmark[4].SetActive(false);
                correspondingCheckmark[5].SetActive(false);
            }
        }
        if (chickenPulled[0] && chickenPulled[1] && chickenPulled[2] && chickenPulled[3]) {
            lvltmp.FinishMinigame(true);
            alreadyOver = true;
        }
    }

    void Start() {
        gusVideo.SetActive(false);
        correspondingCheckmark[0].SetActive(false);
        correspondingCheckmark[1].SetActive(false);
        correspondingCheckmark[2].SetActive(false);
        correspondingCheckmark[3].SetActive(false);
        correspondingCheckmark[4].SetActive(false);
        correspondingCheckmark[5].SetActive(false);
        lvltmp.didWin = false;
        int rng = Random.Range(1, 7);
        chickenReady[0] = rng;
        correspondingCheckmark[rng - 1].SetActive(true);
        rng = Random.Range(1, 7);
        if (rng == chickenReady[0]) {
            rng = (rng % 6) + 1;
        }
        chickenReady[1] = rng;
        correspondingCheckmark[rng - 1].SetActive(true);
        //LosPollosSounds.play("");
        
    }

    void Update() {
        if (cookTimer < 0.0 && cooked && !alreadyOver) {
            cooked = false;
            int rng = Random.Range(1, 7);
            if (rng == chickenReady[0] || rng == chickenReady[1]) {
                rng = (rng % 6) + 1;
                if (rng == chickenReady[0] || rng == chickenReady[1]) {
                    rng = (rng % 6) + 1;
                }
            }
            chickenReady[2] = rng;
            correspondingCheckmark[rng - 1].SetActive(true);
            rng = Random.Range(1, 7);
            if (rng == chickenReady[0] || rng == chickenReady[1] || rng == chickenReady[2]) {
                rng = (rng % 6) + 1;
                if (rng == chickenReady[0] || rng == chickenReady[1] || rng == chickenReady[2]) {
                    rng = (rng % 6) + 1;
                    if (rng == chickenReady[0] || rng == chickenReady[1] || rng == chickenReady[2]) {
                        rng = (rng % 6) + 1;
                    }
                }
            }
            chickenReady[3] = rng;
            correspondingCheckmark[rng - 1].SetActive(true);
            //LosPollosSounds.play("");
        }
        else {
            cookTimer -= Time.deltaTime;
        }
        if(objectsMoving[0] && movementTimer[0] > 0.0) {
            correspondingObject[0].transform.position += new Vector3(0, 0.002f, 0);
            movementTimer[0] -= Time.deltaTime;
        }
        if(objectsMoving[1] && movementTimer[1] > 0.0) {
            correspondingObject[1].transform.position += new Vector3(0, 0.002f, 0);
            movementTimer[1] -= Time.deltaTime;
        }
        if(objectsMoving[2] && movementTimer[2] > 0.0) {
            correspondingObject[2].transform.position += new Vector3(0, 0.002f, 0);
            movementTimer[2] -= Time.deltaTime;
        }
        if(objectsMoving[3] && movementTimer[3] > 0.0) {
            correspondingObject[3].transform.position += new Vector3(0, 0.002f, 0);
            movementTimer[3] -= Time.deltaTime;
        }
        if(objectsMoving[4] && movementTimer[4] > 0.0) {
            correspondingObject[4].transform.position += new Vector3(0, 0.002f, 0);
            movementTimer[4] -= Time.deltaTime;
        }
        if(objectsMoving[5] && movementTimer[5] > 0.0) {
            correspondingObject[5].transform.position += new Vector3(0, 0.002f, 0);
            movementTimer[5] -= Time.deltaTime;
        }
    }

    public IEnumerator endMinigame() {
        //LosPollosSounds.play("");
        yield return new WaitForSeconds(1);
        GameObject badChicken = Instantiate(rawChicken, new Vector3(0, 8, 0), Random.rotation);
        badChicken.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
        badChicken.AddComponent<Rigidbody>();
        yield return new WaitForSeconds(1);
        gusVideo.SetActive(true);
    }
}
