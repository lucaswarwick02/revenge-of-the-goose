using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prologue : MonoBehaviour
{
    [SerializeField] private GameObject[] parts;

    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;
    [SerializeField] private GameObject playButton;

    private int partIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateParts();
    }

    public void LeftArrow () {
        partIndex = Mathf.Clamp(partIndex - 1, 0, parts.Length - 1);

        UpdateParts();
    }

    public void RightArrow () {
        partIndex = Mathf.Clamp(partIndex + 1, 0, parts.Length - 1);

        UpdateParts();
    }

    private void UpdateParts () {
        foreach (GameObject part in parts) {
            part.SetActive(false);
        }

        leftArrow.interactable = partIndex != 0;

        bool onLastPart = partIndex == parts.Length - 1;
        rightArrow.gameObject.SetActive(!onLastPart);
        playButton.SetActive(onLastPart);

        parts[partIndex].SetActive(true);
    }
}
