using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    public string conversationID;
    public List<Dialogue> dialogue;

    public Dictionary<string, Dialogue> CompileDialogue()
    {
        var dict = new Dictionary<string, Dialogue>();
        foreach (Dialogue dialogue in dialogue)
        {
            dict.Add(dialogue.dialogueID, dialogue);
        }
        return dict;
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
