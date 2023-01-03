using System;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    public static event Action<MapArea> OnNextMapAreaChosen;

    [SerializeField] private MapAreaChoice[] gateways;

    private void OnEnable()
    {
        foreach (MapAreaChoice choice in gateways)
        {
            // When the map is loaded, set the choice trigger's to move to the corresponding node
            choice.gateway.OnPlayerEnteredGateway += () => MoveToNextArea(choice.resultingArea);
        }

        OnNextMapAreaChosen += node => { if (node == null) { Debug.LogError("StoryNode not set!"); } else { Debug.Log("Next StoryNode = " + node.name); } };
    }

    private void MoveToNextArea(MapArea newArea)
    {
        OnNextMapAreaChosen?.Invoke(newArea);
    }

    [Serializable]
    public struct MapAreaChoice
    {
        public ChoiceGateway gateway;
        public MapArea resultingArea;
    }
}
