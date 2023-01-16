using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static Conversation;

public class Converser : MonoBehaviour
{
    public static event Action<Transform> OnStartConversation;
    public static event Action OnEndConversation;

    [SerializeField] private float rangeToStartConversation = 3;
    [Space]
    [SerializeField] private List<ConditionalConversation> possibleConversations;
    [SerializeField] private List<ConditionalEffect> endEffects;

    private ExampleUI canvas;
    private DialoguePanel dialoguePanel;

    private Conversation conversation = null;
    private bool conversationStarted;
    private Dictionary<string, Dialogue> dialogueDict;

    [Serializable]
    private struct ConditionalConversation
    {
        public QueryCondition_Base[] conditions;
        public Conversation conversation;
    }

    [Serializable]
    private struct ConditionalEffect
    {
        public Decisions.ResponseQuery responseRequired;
        public UnityEvent effect;
    }

    private void Awake()
    {
        canvas = FindObjectOfType<ExampleUI>();
        ChooseConversation();
    }

    private void Update()
    {
        if (!conversationStarted && (PlayerMovement.Position - transform.position).magnitude <= rangeToStartConversation)
        {
            conversationStarted = true;
            StartConversation();
        }
    }

    private void ChooseConversation()
    {
        conversation = null;
        foreach (var conversation in possibleConversations)
        {
            if (conversation.conditions.All(q => q.Query()))
            {
                this.conversation = conversation.conversation;
                break;
            }
        }

        if (conversation == null)
        {
            Debug.LogError("No conversation conditions were met!! Disabling the converser component.");
            enabled = false;
        }
        else
        {
            Debug.Log("Conversation chosen: " + conversation.conversationID);
            dialogueDict = conversation.CompileDialogue();
        }

    }

    public void StartConversation()
    {
        Decisions.RecordConversationStarted(conversation.conversationID);
        OnStartConversation?.Invoke(transform);
        dialoguePanel = canvas.OpenDialoguePanel();
        EnactDialogue(0);
    }

    private void EnactDialogue(int dialogueIndex)
    {
        EnactDialogue(conversation.dialogue[dialogueIndex]);
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
        Decisions.RecordPlayerResponse(conversation.conversationID, responseID);

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
                if (Decisions.Query(effect.responseRequired))
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

    public void TurnAnimals()
    {
        PlaythroughStats.AnimalsTurned = true;
    }
}
