using TMPro;
using UnityEngine;

public class ExampleUI : MonoBehaviour
{
    [Header("Destruction Scoring")]
    [SerializeField] private TMP_Text destructionScoreText;
    [SerializeField] private DestructionScorePopup scorePopupPrefab;

    void OnEnable()
    {
        PlaythroughStats.OnDestructionScoreChanged += UpdateDestructionScore;
    }

    void OnDisable()
    {
        PlaythroughStats.OnDestructionScoreChanged -= UpdateDestructionScore;
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
}
