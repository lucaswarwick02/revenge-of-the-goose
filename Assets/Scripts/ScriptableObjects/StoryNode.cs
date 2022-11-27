using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New StoryNode", menuName = "StoryNode")]
public class StoryNode : ScriptableObject
{
    public class NodeEvent : UnityEvent<StoryNode> {}
    public static NodeEvent OnNodeChange = new NodeEvent();

    [Header("Visuals")]
    public Sprite groundSprite;

    [Header("Choices")]
    public string prompt;
    public Choice choice1;
    public Choice choice2;
}

[System.Serializable]
public class Choice {
    public string text;
    public StoryNode linkedNode;
}
