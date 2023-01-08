using System;
using UnityEngine;
using static PlaythroughStats;

public class MapArea : MonoBehaviour
{
    public static event Action<MapArea> OnNextMapAreaChosen;

    [SerializeField] public bool NeutralArea;
    [SerializeField] private MapAreaChoice[] gateways;

    public int phase;

    private void OnEnable()
    {
        foreach (MapAreaChoice choice in gateways)
        {
            // When the map is loaded, set the choice trigger's to move to the corresponding node
            choice.gateway.OnPlayerEnteredGateway += () => MoveToNextArea(choice.thresholdOptions);
        }

        OnNextMapAreaChosen += node => { if (node == null) { Debug.LogError("StoryNode not set!"); } else { Debug.Log("Next StoryNode = " + node.name); } };
    }

    private void MoveToNextArea(ThresholdOption[] thresholdOptions)
    {
        foreach (ThresholdOption thresholdOption in thresholdOptions) {
            if (PlaythroughStats.Query(thresholdOption.query)) {
                OnNextMapAreaChosen?.Invoke(thresholdOption.resultingArea);
                break;
            }
        }
    }

    [Serializable]
    public struct MapAreaChoice
    {
        public ChoiceGateway gateway; // The physical gateway itself
        public ThresholdOption[] thresholdOptions;
    }

    [Serializable]
    public struct ThresholdOption
    {
        public MapArea resultingArea; // The next map area to be loaded
        public StatisticQuery query;
    }
}
