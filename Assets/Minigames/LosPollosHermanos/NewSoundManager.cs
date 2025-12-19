using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSoundManager : MonoBehaviour {

    private static NewSoundManager Instance;
    private static AudioSource source;
    private static NewSoundEffects sounds;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            source = GetComponent<AudioSource>();
            sounds = GetComponent<NewSoundEffects>();
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public static void playTheAudio(string name) {
        AudioClip audio = sounds.returnRandom(name);
        if (audio != null) {
            source.PlayOneShot(audio);
        }
    }
}
