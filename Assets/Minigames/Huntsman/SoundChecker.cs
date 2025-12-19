using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundChecker : MonoBehaviour {
    
    public float soundSensibility = 1000;
    public float threshold = 0.1f;
    public int window = 64;
    public Vector3 minScale;
    public Vector3 maxScale;
    private AudioClip microphoneAudioClip;
    [SerializeField] private Material bodyColor;
    [SerializeField] private Material brightRed;
    public Renderer[] renderers = new Renderer[8];
    [SerializeField] LevelTemplate lvltmp;
    public bool alreadyOver = false;
    public UnityEvent explosion;
    public GameObject huntsman1;
    public GameObject huntsman2;
    public UnityEvent huntsmanshot;
    public GameObject laserControl;
    public GameObject currentSemibot;
    public GameObject huntsmanVideo;
    // Start is called before the first frame update
    void Start() {
        huntsmanVideo.SetActive(false);
        huntsman2.SetActive(false);
        huntsman1.SetActive(true);
        MicrophoneToAudioClip();
        lvltmp.didWin = true;
        renderers[0].material = bodyColor;
        renderers[1].material = bodyColor;
        renderers[2].material = bodyColor;
        renderers[3].material = bodyColor;
        renderers[4].material = bodyColor;
        renderers[5].material = bodyColor;
        renderers[6].material = bodyColor;
        renderers[7].material = bodyColor;
    }

    // Update is called once per frame
    void Update() {
        if (lvltmp.timer < 0 && !alreadyOver) {
            alreadyOver = true;
            huntsman2.SetActive(false);
            huntsman1.SetActive(false);
            huntsmanVideo.SetActive(true);
        }
        float audioStrength = soundSensibility * returnAudioStrength(Microphone.GetPosition(Microphone.devices[0]), microphoneAudioClip);
        if (audioStrength > threshold && !alreadyOver) {
            alreadyOver = true;
            StartCoroutine(Explode());
            lvltmp.FinishMinigame(false);
        }
    }

    public void MicrophoneToAudioClip() {
        string microphoneName = Microphone.devices[0];
        microphoneAudioClip = Microphone.Start(microphoneName, true, 7, AudioSettings.outputSampleRate);
    }

    public float returnAudioStrength(int audioPosition, AudioClip clip) {
        int startPosition = audioPosition - window;
        if (startPosition < 0) {
            return 0;
        }
        float[] waveData = new float[window];
        clip.GetData(waveData, startPosition);
        float audioStrength = 0;
        for (int i = 0; i < window; i++) {
            audioStrength += Mathf.Abs(waveData[i]);
        }
        return audioStrength / window;
    }

    public IEnumerator Explode() {
        huntsman1.SetActive(false);
        huntsman2.SetActive(true);
        SoundManager.play("HuntsmanShoot");
        yield return new WaitForSeconds(1);
        huntsmanshot.Invoke();
        SoundManager.play("HuntsmanFiring");
        SoundManager.play("Death");
        renderers[0].material = brightRed;
        renderers[1].material = brightRed;
        renderers[2].material = brightRed;
        renderers[3].material = brightRed;
        renderers[4].material = brightRed;
        renderers[5].material = brightRed;
        renderers[6].material = brightRed;
        renderers[7].material = brightRed;
        yield return new WaitForSeconds(0.05f);
        laserControl.SetActive(false);
        yield return new WaitForSeconds(0.9f);
        explosion.Invoke();
        SoundManager.play("SemibotExplosion");
        Destroy(gameObject);
    }

}
