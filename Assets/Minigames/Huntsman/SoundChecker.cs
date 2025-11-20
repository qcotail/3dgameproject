using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundChecker : MonoBehaviour {
    
    public float soundSensibility = 1000;
    public float threshold = 0.1f;
    public int window = 64;
    public Vector3 minScale;
    public Vector3 maxScale;
    private AudioClip microphoneAudioClip;
    //something something levelTemplate lvltmpl;

    // Start is called before the first frame update
    void Start() {
        MicrophoneToAudioClip();
    }

    // Update is called once per frame
    void Update() {
        float audioStrength = soundSensibility * returnAudioStrength(Microphone.GetPosition(Microphone.devices[0]), microphoneAudioClip);
        if (audioStrength < threshold) {
            audioStrength = 0;
            //lvltmpl.didWin = true;
        }
        transform.localScale = Vector3.Lerp(minScale, maxScale, audioStrength);
        //lvltmpl.didWin = false;
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

}
