using Game.Utility;
using TMPro;
using UnityEngine;

public class ExampleUI : MonoBehaviour
{
    //public static Vector3 CurrentScoreTextWorldPos { get; private set; }

    [SerializeField] private StoryNode startNode;

    [Header("Destruction Scoring")]
    [SerializeField] private TMP_Text destructionScoreText;
    [SerializeField] private DestructionScorePopup scorePopupPrefab;

    void OnEnable()
    {
        StoryNode.OnCurrentStoryNodeChange += UpdateUI;
        StoryNode.CurrentStoryNode = startNode;

        PlaythroughStats.OnDestructionScoreChanged += UpdateDestructionScore;
    }

    void OnDisable()
    {
        StoryNode.OnCurrentStoryNodeChange -= UpdateUI;
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

    public void UpdateUI(StoryNode node)
    {
    }
}
