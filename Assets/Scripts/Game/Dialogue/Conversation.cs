using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    public string conversationID;
    public List<Dialogue> dialogue;
    
    [HideInInspector] public readonly Dictionary<string, Dialogue> dialogueDict = new Dictionary<string, Dialogue>();

    public void CompileDialogue()
    {
        foreach (Dialogue dialogue in dialogue)
        {
            dialogueDict.Add(dialogue.dialogueID, dialogue);
        }
    }

    [Serializable]
    public struct Dialogue
    {
        public string dialogueID;
        public string text;
        public float secondsShownFor;
        public List<Response> responsesToWaitFor;
        public string nextDialogueID;
    }

    [Serializable]
    public struct Response
    {
        public string responseID;
        public string text;
        public string nextDialogueID;
    }
}
