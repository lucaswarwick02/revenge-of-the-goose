using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{
    [SerializeField] private Transform explosionImage;
    [SerializeField] private UnityEvent callbacks;

    private void Awake()
    {
        explosionImage.localScale = Vector3.zero;
    }

    public void Trigger()
    {
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
        StartCoroutine(DoAnimation());
    }

    private IEnumerator DoAnimation()
    {
        float timePassed = 0;
        while (timePassed <= 3)
        {
            explosionImage.localScale = Vector3.one * timePassed * 10;
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }
        callbacks?.Invoke();
    }
}
