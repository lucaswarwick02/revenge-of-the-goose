using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Prologue : MonoBehaviour
{
    [SerializeField] private GameObject[] parts;
    [Space]
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [SerializeField] private GameObject playButton;
    [Space]
    [SerializeField] int gameSceneIndex;
    [Space]
    [SerializeField] private GameObject fade;
    [SerializeField] private float fadeDuration = 2f;

    private int partIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateParts();

        BackgroundMusic.ForceNeutralMusic();

        StartCoroutine("fadeFromWhite", fade.GetComponent<Image>());
    }

    public void LeftArrow () {
        partIndex = Mathf.Clamp(partIndex - 1, 0, parts.Length - 1);

        UpdateParts();
    }

    public void RightArrow () {
        partIndex = Mathf.Clamp(partIndex + 1, 0, parts.Length - 1);

        UpdateParts();
    }

    private void UpdateParts () {
        foreach (GameObject part in parts) {
            part.SetActive(false);
        }

        leftArrow.interactable = partIndex != 0;

        bool onLastPart = partIndex == parts.Length - 1;
        rightArrow.gameObject.SetActive(!onLastPart);
        playButton.SetActive(onLastPart);

        parts[partIndex].SetActive(true);
    }

    public void Play ()
    {
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
}
