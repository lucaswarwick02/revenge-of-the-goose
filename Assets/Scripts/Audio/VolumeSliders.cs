using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSliders : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundsSlider;

    private void Start() {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SetMusicVolume(musicSlider.value);

        soundsSlider.value = PlayerPrefs.GetFloat("SoundsVolume", 0.5f);
        SetSoundsVolume(soundsSlider.value);
    }

    public void SetMusicVolume (float value) {
        PlayerPrefs.SetFloat("MusicVolume", value);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSoundsVolume (float value) {
        PlayerPrefs.SetFloat("SoundsVolume", value);
        audioMixer.SetFloat("SoundsVolume", Mathf.Log10(value) * 20);
    }
}
