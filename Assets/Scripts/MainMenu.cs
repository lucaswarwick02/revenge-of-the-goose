using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int gameSceneIndex;

    [SerializeField] private PlayableDirector intro;

    [Header("Background Music")]
    [SerializeField] BackgroundMusic backgroundMusic;
    [SerializeField] AudioClip musicClip;

    private void Start() {
        Invoke("EnableMusic", 4f);
    }

    private void Update() {
        if (intro.time < 5.9f && Input.GetKeyDown(KeyCode.Escape)) {
            intro.time = 6f;
        }
    }

    public void Exit ()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Play ()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    void EnableMusic () {
        BackgroundMusic.PlayMusic(musicClip);
    }
}
