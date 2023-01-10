using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.UI;
using System;

public class MainMenu : MonoBehaviour
{
    public static event Action OnMainMenuStarted;
    public static event Action OnMainMenuAnimationComplete;
    public static event Action OnMainMenuAnimationSoftComplete;

    [SerializeField] int prologueSceneIndex;
    [SerializeField] private PlayableDirector intro;
    [Space]
    [SerializeField] private GameObject fade;
    [SerializeField] private float fadeDuration = 2f;

    private void Start()
    {
        BackgroundMusic.StopMusic();
        OnMainMenuStarted?.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        StartCoroutine("fadeToWhite", fade.GetComponent<Image>());
    }

    public void AnimationEnd()
    {
        OnMainMenuAnimationComplete?.Invoke();
    }

    public void AnimationSoftEnd()
    {
        OnMainMenuAnimationSoftComplete?.Invoke();
    }

    IEnumerator fadeToWhite(Image image)
    {
        float counter = 0;
        Color spriteColor = image.color;

        fade.SetActive(true);

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            image.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Mathf.Lerp(0, 1, counter / fadeDuration));

            yield return null;
        }

        SceneManager.LoadScene(prologueSceneIndex);
    }
}
