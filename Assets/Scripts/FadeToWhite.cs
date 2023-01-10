using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToWhite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer fadeToWhiteSprite;
    [SerializeField] private float fadeToWhiteDelay;
    [SerializeField] private float fadeToWhiteDuration;

    private void Awake()
    {
        fadeToWhiteSprite.enabled = false;
    }

    public void TriggerAnimation()
    {
        StartCoroutine(FadeToWhiteRoutine());
    }

    private IEnumerator FadeToWhiteRoutine()
    {
        yield return new WaitForSecondsRealtime(fadeToWhiteDelay);
        fadeToWhiteSprite.enabled = true;
        float timePassed = 0;
        while (fadeToWhiteSprite.color.a < 1)
        {
            fadeToWhiteSprite.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timePassed / fadeToWhiteDuration);
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }
    }
}
