using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Story Node", menuName = "Game/Story Node")]
public class StoryNode : ScriptableObject
{
    private static StoryNode currentStoryNode = null;

    public static StoryNode CurrentStoryNode
    {
        get => currentStoryNode;
        set
        {
            currentStoryNode = value;
            OnCurrentStoryNodeChange?.Invoke(currentStoryNode);
        }

    }

    public static Action<StoryNode> OnCurrentStoryNodeChange;

    [Header("Choices")]
    public string Prompt;
    public Choice[] Choices;

    [Header("Visuals")]
    public Sprite GroundSprite;
}
