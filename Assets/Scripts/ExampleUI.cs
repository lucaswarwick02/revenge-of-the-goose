using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExampleUI : MonoBehaviour
{
    [Header("HUD")]
    [SerializeField] private TMP_Text destructionScoreText;
    [SerializeField] private DestructionScorePopup scorePopupPrefab;
    [SerializeField] private Slider gooseHealthBar; 

    [Header("UI Panels")]
    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private GameOverPanel gameOverPanel;
    [SerializeField] private DialoguePanel dialoguePanel;

    [Header("Fade")]
    [SerializeField] private GameObject fade;
    [SerializeField] private float fadeDuration = 2f;

    [Header("New Map Area Animation")]
    [SerializeField] private Image screenDarkenImage;

    private void Start() {
        StartCoroutine("fadeFromWhite", fade.GetComponent<Image>());
    }


    void OnEnable()
    {
        PlaythroughStats.OnDestructionScoreChanged += UpdateDestructionScore;
        GameHandler.OnGameLoaded += UpdateDestructionScore;
        GameHandler.OnGameOver += OnGameOver;

        GameHandler.OnMapAreaStartChanging += DarkenScreen;
        GameHandler.OnMapAreaChanged += UndarkenScreen;
    }

    void OnDisable()
    {
        PlaythroughStats.OnDestructionScoreChanged -= UpdateDestructionScore;
        GameHandler.OnGameLoaded -= UpdateDestructionScore;
        GameHandler.OnGameOver -= OnGameOver;

        GameHandler.OnMapAreaStartChanging -= DarkenScreen;
        GameHandler.OnMapAreaChanged -= UndarkenScreen;
    }

    private void UpdateDestructionScore(int newScore, int deltaScore, Vector3 destructionEventPosition)
    {
        if (deltaScore > 0)
        {
            DestructionScorePopup scorePopup = Instantiate(scorePopupPrefab, transform);
            scorePopup.Initialise(deltaScore, destructionEventPosition, (Vector2)destructionScoreText.transform.position, () => SetDestructionScore(newScore));
        }
    }

    private void UpdateDestructionScore()
    {
        SetDestructionScore(PlaythroughStats.DestructionScore);
    }

    private void SetDestructionScore(int score)
    {
        destructionScoreText.text = $"{score}";
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pausePanel.Pause();
        }

        gooseHealthBar.value = PlayerHealth.CurrentHealth / PlayerHealth.MAX_HEALTH;
    }

    private void OnGameOver(Transform killer)
    {
        gameOverPanel.gameObject.SetActive(true);
    }

    public DialoguePanel OpenDialoguePanel()
    {
        dialoguePanel.gameObject.SetActive(true);
        return dialoguePanel;
    }

    public void CloseDialoguePanel()
    {
        dialoguePanel.gameObject.SetActive(false);
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

    private void DarkenScreen()
    {
        if (screenDarkenImage.color.a == 0)
        {
            StartCoroutine(ScreenDarkenImageFade(0, 1, 0.5f));
        }
    }

    private void UndarkenScreen()
    {
        if (screenDarkenImage.color.a == 1)
        {
            StartCoroutine(ScreenDarkenImageFade(1, 0, 0.5f));
        }
    }

    private IEnumerator ScreenDarkenImageFade(float fromOpacity, float toOpacity, float duration)
    {
        float timePassed = 0;
        while (screenDarkenImage.color.a != toOpacity)
        {
            screenDarkenImage.color = new Color(screenDarkenImage.color.r, screenDarkenImage.color.g, screenDarkenImage.color.b, Mathf.Lerp(fromOpacity, toOpacity, timePassed / duration));
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
