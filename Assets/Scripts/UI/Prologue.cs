using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    [SerializeField] private GameObject[] parts;
    [SerializeField] private GameObject playButton;
    [SerializeField] private Slider progressBar;
    [Space]
    [SerializeField] int gameSceneIndex;
    [Space]
    [SerializeField] private GameObject fade;
    [SerializeField] private float fadeDuration = 2f;

    private int partIndex = 0;

    private float partTime = 7f;
    private float currentTime;

    private bool isFadeActive = true;

    // Start is called before the first frame update
    void Start()
    {
        UpdateParts();

        currentTime = partTime;

        BackgroundMusic.ForceNeutralMusic();

        StartCoroutine("fadeFromWhite", fade.GetComponent<Image>());
    }

    private void Update() {
        if (isFadeActive) return;

        currentTime -= Time.deltaTime;
        UpdateProgressBar();
        if (currentTime < 0) {
            partIndex++;
            currentTime = partTime;
            UpdateParts();
        }
    }

    private void UpdateParts () {
        if (partIndex == parts.Length)
        {
            Play();
            return;
        }

        foreach (GameObject part in parts) {
            part.SetActive(false);
        }

        parts[partIndex].SetActive(true);
    }

    public void Play ()
    {
        progressBar.gameObject.SetActive(false);
        StartCoroutine("fadeToWhite", fade.GetComponent<Image>());
    }

    IEnumerator fadeFromWhite(Image image)
    {
        float counter = 0;
        Color spriteColor = image.color;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            image.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Mathf.Lerp(1, 0, counter / fadeDuration));

            yield return null;
        }

        fade.SetActive(false);
        isFadeActive = false;
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

        SceneManager.LoadScene(gameSceneIndex);
    }

    private void UpdateProgressBar () {
        progressBar.value = 1f - (currentTime / partTime);
    }
}
