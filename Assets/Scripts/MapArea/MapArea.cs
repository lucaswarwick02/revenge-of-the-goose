using System;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] private Transform startPoint; // ! Depricated
    [SerializeField] private Transform choiceCameraSnapPoint; // ! Depricated
    [SerializeField] private MapAreaChoice[] choices;

    public Action<Vector3> OnChoiceTriggered;
    public Action<StoryNode> OnNextStoryNodeChosen;

    private void OnEnable()
    {
        foreach (MapAreaChoice choice in choices)
        {
            // When the map is loaded, set the choice trigger's to move to the corresponding node
            choice.gateway.OnPlayerEnteredGateway += () => MoveToNextStoryNode(choice.resultingNode);
        }

        OnNextStoryNodeChosen += node => { if (node == null) { Debug.LogError("StoryNode not set!"); } else { Debug.Log("Next StoryNode = " + node.name); } };
    }

    private void StartChoice()
    {
        OnChoiceTriggered?.Invoke(choiceCameraSnapPoint.position); // ! Depricated
    }

    private void MoveToNextStoryNode(StoryNode newNode)
    {
        OnNextStoryNodeChosen?.Invoke(newNode);
    }

    [Serializable]
    public struct MapAreaChoice
    {
        public ChoiceGateway gateway;
        public StoryNode resultingNode;
    }
}
