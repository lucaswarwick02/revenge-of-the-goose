using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StoryNode", menuName = "StoryNode")]
public class StoryNode : ScriptableObject
{
    public string prompt;
    public Choice choice1;
    public Choice choice2;
}

[System.Serializable]
public class Choice {
    public string text;
    public StoryNode linkedNode;
}
