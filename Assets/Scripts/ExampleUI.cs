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


    void OnEnable()
    {
        PlaythroughStats.OnDestructionScoreChanged += UpdateDestructionScore;
        GameHandler.OnGameOver += OnGameOver;
    }

    void OnDisable()
    {
        PlaythroughStats.OnDestructionScoreChanged -= UpdateDestructionScore;
        GameHandler.OnGameOver -= OnGameOver;
    }

    private void UpdateDestructionScore(int newScore, int deltaScore, Vector3 destructionEventPosition)
    {
        if (deltaScore > 0)
        {
            DestructionScorePopup scorePopup = Instantiate(scorePopupPrefab, transform);
            scorePopup.Initialise(deltaScore, destructionEventPosition, (Vector2)destructionScoreText.transform.position, () => SetDestructionScore(newScore));
        }
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

    private void OnGameOver(Vector3 deathCausePosition)
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
}
