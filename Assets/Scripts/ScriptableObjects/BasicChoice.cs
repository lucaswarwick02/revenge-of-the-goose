using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Basic Choice", menuName = "Game/Basic Choice")]
public class BasicChoice : Choice
{
    public StoryNode NextStoryNode;
}
