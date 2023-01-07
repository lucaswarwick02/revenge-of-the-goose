using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    private const float ANIMATION_TIME = 0.5f;

    [SerializeField] private Transform dialogueBox;
    [SerializeField] private Transform responseBox;
    [SerializeField] private Button responsePrefab;

    private TMP_Text dialogueText;

    private void Awake()
    {
        dialogueText = dialogueBox.GetComponentInChildren<TMP_Text>();
        dialogueBox.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameHandler.SetPaused(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnDisable()
    {
        GameHandler.SetPaused(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OpenDialogue(string text, float secondsShownFor, Action onComplete)
    {
        StartCoroutine(OpenDialogueRoutine(text, secondsShownFor, onComplete));
    }

    public void CloseDialogue(Action onComplete)
    {
        StartCoroutine(CloseDialogueRoutine(onComplete));
    }

    public void ShowResponses(List<Tuple<string, Action>> responses)
    {
        foreach (Tuple<string, Action> response in responses)
        {
            Button newResponse = Instantiate(responsePrefab, responseBox);
            newResponse.GetComponentInChildren<TMP_Text>().text = response.Item1;
            newResponse.onClick.AddListener(() => response.Item2());
        }
    }

    private IEnumerator OpenDialogueRoutine(string text, float secondsShownFor, Action onComplete)
    {
        // Animate out if the box is currently showing dialogue
        if (dialogueBox.localScale == Vector3.one)
        {
            yield return CloseDialogueRoutine();
        }

        float timePassed = 0;

        dialogueText.text = text;

        // Animate dialogue in
        while (timePassed <= ANIMATION_TIME)
        {
            dialogueBox.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timePassed / ANIMATION_TIME);
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        dialogueBox.localScale = Vector3.one;

        yield return new WaitForSecondsRealtime(secondsShownFor);

        onComplete?.Invoke();
    }



    private IEnumerator CloseDialogueRoutine(Action onComplete = null)
    {
        // remove any responses
        RemoveResponses();

        float timePassed = 0;

        // Animate dialogue out
        while (timePassed <= ANIMATION_TIME)
        {
            dialogueBox.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timePassed / ANIMATION_TIME);
            timePassed += Time.unscaledDeltaTime;
            yield return null;
        }

        dialogueBox.localScale = Vector3.zero;

        onComplete?.Invoke();
    }

    private void RemoveResponses()
    {
        foreach (Transform child in responseBox)
        {
            Destroy(child.gameObject);
        }
    }
}
