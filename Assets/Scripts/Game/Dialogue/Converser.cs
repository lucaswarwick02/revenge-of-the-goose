using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Converser : MonoBehaviour
{
    public static event Action<Vector3> OnStartConversation;
    public static event Action OnEndConversation;

    [SerializeField] private string conversationID;
    [SerializeField] private float rangeToStartConversation = 3;
    [Space]
    [SerializeField] private List<Dialogue> dialogue;
    [SerializeField] private List<ConditionalEffect> endEffects;

    private ExampleUI canvas;
    private DialoguePanel dialoguePanel;

    private readonly Dictionary<string, Dialogue> dialogueDict = new Dictionary<string, Dialogue>();
    private bool conversationStarted;

    [Serializable]
    private struct Dialogue
    {
        public string dialogueID;
        public string text;
        public float secondsShownFor;
        public List<Response> responsesToWaitFor;
        public string nextDialogueID;
    }

    [Serializable]
    private struct Response
    {
        public string responseID;
        public string text;
        public string nextDialogueID;
    }

    [Serializable]
    private struct ConditionalEffect
    {
        public string responseRequired;
        public UnityEvent effect;
    }

    private void Awake()
    {
        canvas = FindObjectOfType<ExampleUI>();

        foreach (Dialogue dialogue in dialogue)
        {
            dialogueDict.Add(dialogue.dialogueID, dialogue);
        }
    }
    private void Update()
    {
        if (!conversationStarted && (PlayerMovement.Position - transform.position).magnitude <= rangeToStartConversation)
        {
            conversationStarted = true;
            StartConversation();
        }
    }

    public void StartConversation()
    {
        OnStartConversation?.Invoke(transform.position);
        dialoguePanel = canvas.OpenDialoguePanel();
        EnactDialogue(0);
    }

    private void EnactDialogue(int dialogueIndex)
    {
        EnactDialogue(dialogue[dialogueIndex]);
    }

    private void EnactDialogue(string dialogueID)
    {
        if (dialogueID == string.Empty)
        {
            EndConversation();
        }
        else
        {
            EnactDialogue(dialogueDict[dialogueID]);
        }
    }

    private void EnactDialogue(Dialogue dialogue)
    {
        if (dialogue.responsesToWaitFor.Count > 0)
        {
            dialoguePanel.OpenDialogue(dialogue.text, dialogue.secondsShownFor, () =>
            {
                dialoguePanel.ShowResponses(dialogue.responsesToWaitFor
                    .Select(response => new Tuple<string, Action>(response.text, () => RecieveResponse(response.responseID, response.nextDialogueID)))
                    .ToList());
            });
        }
        else
        {
            dialoguePanel.OpenDialogue(dialogue.text, dialogue.secondsShownFor, () =>
            {
                EnactDialogue(dialogue.nextDialogueID);
            });
        }
    }

    private void RecieveResponse(string responseID, string nextDialogueID)
    {
        // Record response for use later in story
        Decisions.RecordPlayerResponse(conversationID, responseID);

        // Move to next dialogue
        if (nextDialogueID == string.Empty)
        {
            EndConversation();
        }
        else
        {
            EnactDialogue(nextDialogueID);
        }
    }

    private void EndConversation()
    {
        dialoguePanel.CloseDialogue(() =>
        {
            canvas.CloseDialoguePanel();

            foreach (ConditionalEffect effect in endEffects)
            {
                if (effect.responseRequired == string.Empty || Decisions.PlayerResponded(conversationID, effect.responseRequired))
                {
                    effect.effect?.Invoke();
                }
            }

            GameHandler.SetPaused(false);
            OnEndConversation?.Invoke();
        });
    }

    public void EnableHostile()
    {
        GameHandler.SetNeutralMode(false);
    }
}
