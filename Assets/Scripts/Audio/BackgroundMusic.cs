using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    static BackgroundMusic INSTANCE;

    AudioSource audioSource;

    private void Awake() {
        INSTANCE = this;

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlayMusic () {
        if (INSTANCE.audioSource.isPlaying) return;
        INSTANCE.audioSource.Play();
    }

    public static void StopMusic () {
        INSTANCE.audioSource.Stop();
    }
}
