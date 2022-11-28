using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minigame Choice", menuName = "Game/Minigame Choice")]
public class MinigameChoice : Choice
{
    public string MinigameScene;
    public StoryNode NextStoryNodeIfWin;
    public StoryNode NextStoryNodeIfLose;
}
