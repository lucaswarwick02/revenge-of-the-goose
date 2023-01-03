using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int gameSceneIndex;

    [Header("Background Music")]
    [SerializeField] BackgroundMusic backgroundMusic;
    [SerializeField] AudioClip musicClip;

    private void Start() {
        Invoke("EnableMusic", 4f);
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
