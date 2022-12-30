using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DestructionScorePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private AnimationCurve popupYPosAnimationCurve;
    [SerializeField] private AnimationCurve popupScaleAnimationCurve;
    [SerializeField] private AnimationCurve mergeAnimationCurve;
    [SerializeField] private AnimationCurve mergeScaleAnimationCurve;

    private Vector3 destructionEventPosition;
    private Vector2 destructionScoreTextScreenPos;
    private Action onMerged;

    private Vector3 worldPosAfterPopup;

    public void Initialise(int scoreToDisplay, Vector3 destructionEventPosition, Vector2 destructionScoreTextScreenPos, Action onMerged)
    {
        scoreText.text = $"{scoreToDisplay}";
        this.destructionEventPosition = destructionEventPosition;
        this.destructionScoreTextScreenPos = destructionScoreTextScreenPos;
        this.onMerged = onMerged;

        StartCoroutine(PopupAnimation());
    }

    private IEnumerator PopupAnimation()
    {
        float duration = popupYPosAnimationCurve.keys[^1].time;
        float timePassed = 0;
        Vector3 upDir = Quaternion.Euler(30, 0, 0) * Vector3.up;

        while (enabled && timePassed <= duration)
        {
            worldPosAfterPopup = destructionEventPosition + upDir * popupYPosAnimationCurve.Evaluate(timePassed);
            transform.position = Camera.main.WorldToScreenPoint(worldPosAfterPopup);
            transform.localScale = Vector2.one * popupScaleAnimationCurve.Evaluate(timePassed / duration);

            timePassed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(MergeWithTotalScoreAnimation());
    }

    private IEnumerator MergeWithTotalScoreAnimation()
    {
        float duration = mergeAnimationCurve.keys[^1].time;
        float timePassed = 0;

        while (enabled && timePassed <= duration)
        {
            Vector2 destructionEventScreenPos = Camera.main.WorldToScreenPoint(worldPosAfterPopup);
            transform.position = Vector2.Lerp(destructionEventScreenPos, destructionScoreTextScreenPos, mergeAnimationCurve.Evaluate(timePassed));
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, mergeScaleAnimationCurve.Evaluate(timePassed / duration));

            timePassed += Time.deltaTime;
            yield return null;
        }

        onMerged?.Invoke();
        Destroy(gameObject);
    }
}
