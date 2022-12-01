using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float sizeIncrease = 1.15f;
    float fadeDuration = 0.15f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(IncreaseFontSize());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(DecreaseFontSize());
    }

    IEnumerator IncreaseFontSize()
    {
        float t = 0;
        while (t < 1)
        {
            // Now the loop will execute on every end of frame until the condition is true
            transform.localScale = Vector3.one * Mathf.Lerp(1, sizeIncrease, t);
            t += Time.deltaTime / fadeDuration;
            yield return new WaitForEndOfFrame(); // So that I return something at least.
        }
    }

    IEnumerator DecreaseFontSize()
    {
        float t = 0;
        while (t < 1)
        {
            // Now the loop will execute on every end of frame until the condition is true
            transform.localScale = Vector3.one * Mathf.Lerp(sizeIncrease, 1, t);
            t += Time.deltaTime / fadeDuration;
            yield return new WaitForEndOfFrame(); // So that I return something at least.
        }
    }
}
