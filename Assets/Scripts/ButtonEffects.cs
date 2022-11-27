using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI textMesh;

    float originalSize;

    float sizeIncrease = 1.15f;
    float fadeDuration = 0.15f;

    private void Awake() {
        textMesh = GetComponent<TextMeshProUGUI>();
        originalSize = textMesh.fontSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartCoroutine(IncreaseFontSize());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(DecreaseFontSize());
    }

    IEnumerator IncreaseFontSize()
    {
        float t = 0;
        while (t < 1)
        {
            // Now the loop will execute on every end of frame until the condition is true
            textMesh.fontSize = Mathf.Lerp(originalSize, originalSize * sizeIncrease, t);
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
            textMesh.fontSize = Mathf.Lerp(originalSize * sizeIncrease, originalSize, t);
            t += Time.deltaTime / fadeDuration;
            yield return new WaitForEndOfFrame(); // So that I return something at least.
        }
    }
}
