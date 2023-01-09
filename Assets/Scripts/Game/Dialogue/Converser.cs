using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static Conversation;

public class Converser : MonoBehaviour
{
    public static event Action<Vector3> OnStartConversation;
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
        public PlaythroughStats.StatisticQuery[] conditions;
        public ResponseQuery[] resonsesRequired;
        public Conversation conversation;
    }

    [Serializable]
    private struct ResponseQuery
    {
        public string conversation;
        public string response;
    }

    [Serializable]
    private struct ConditionalEffect
    {
        public ResponseQuery responseRequired;
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
            if (conversation.conditions.All(c => PlaythroughStats.Query(c)) 
                && conversation.resonsesRequired.All(r => Decisions.PlayerResponded(r.conversation, r.response)))
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
        OnStartConversation?.Invoke(transform.position);
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
                if (effect.responseRequired.conversation == string.Empty
                    || (effect.responseRequired.response == string.Empty && Decisions.ConversationWasStarted(effect.responseRequired.conversation))
                    || Decisions.PlayerResponded(conversation.conversationID, effect.responseRequired.response))
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
