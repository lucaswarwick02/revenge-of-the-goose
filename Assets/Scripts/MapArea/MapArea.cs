using System;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform choiceCameraSnapPoint;
    [SerializeField] private MapAreaChoice[] choices;

    public Action<Vector3> OnChoiceTriggered;
    public Action<StoryNode> OnNextStoryNodeChosen;

    private void OnEnable()
    {
        foreach (MapAreaChoice choice in choices)
        {
            choice.gateway.OnPlayerEnteredGateway += () => MoveToNextStoryNode(choice.resultingNode);
        }
    }

    private void StartChoice()
    {
        OnChoiceTriggered?.Invoke(choiceCameraSnapPoint.position);
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
