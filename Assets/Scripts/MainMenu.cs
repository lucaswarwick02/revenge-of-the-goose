using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using System;

public class MainMenu : MonoBehaviour
{
    public static event Action OnMainMenuStarted;
    public static event Action OnMainMenuAnimationComplete;
    public static event Action OnMainMenuAnimationSoftComplete;

    [SerializeField] int gameSceneIndex;
    [SerializeField] private PlayableDirector intro;

    private void Start()
    {
        OnMainMenuStarted?.Invoke();
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

    public void AnimationEnd()
    {
        OnMainMenuAnimationComplete?.Invoke();
    }

    public void AnimationSoftEnd()
    {
        OnMainMenuAnimationSoftComplete?.Invoke();
    }
}
