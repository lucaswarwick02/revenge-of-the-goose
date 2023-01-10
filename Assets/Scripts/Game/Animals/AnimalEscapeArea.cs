using System.Collections;
using UnityEngine;

public class AnimalEscapeArea : MonoBehaviour
{
    [SerializeField] private float escapeAreaWidth;
    [SerializeField] private SpriteRenderer fadeToWhiteSprite;
    [SerializeField] private float fadeToWhiteDelay;
    [SerializeField] private float fadeToWhiteDuration;

    public Vector3 LineStart => transform.position - Vector3.right * escapeAreaWidth / 2;

    public Vector3 LineEnd => transform.position + Vector3.right * escapeAreaWidth / 2;

    public void TriggerAnimalEscape()
    {
        GetComponentInChildren<SpriteRenderer>().color = Color.clear;
        GameHandler.SetNeutralMode(true);
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
        foreach (AnimalEscapee animal in FindObjectsOfType<AnimalEscapee>())
        {
            animal.TriggerEscape(this);
        }

        StartCoroutine(FadeToWhite());
    }

    private IEnumerator FadeToWhite()
    {
        yield return new WaitForSecondsRealtime(fadeToWhiteDelay);
        float timePassed = 0;
        while (fadeToWhiteSprite.color.a < 1)
        {
            fadeToWhiteSprite.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, timePassed / fadeToWhiteDuration);
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(LineStart, LineEnd);
    }
#endif
}
