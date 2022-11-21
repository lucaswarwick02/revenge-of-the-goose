using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoryNode", menuName = "StoryNode")]
public class StoryNode : ScriptableObject
{
    public string prompt;
    public Choice[] choices;
}

[System.Serializable]
public class Choice {
    public string text;
    public StoryNode linkedNode;
}
