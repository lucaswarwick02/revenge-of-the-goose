using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static PlaythroughStats;

public class MapArea : MonoBehaviour
{
    public static event Action<MapArea> OnNextMapAreaChosen;

    [SerializeField] public bool NeutralArea;
    [SerializeField] public bool IsCheckpoint;
    [SerializeField] private MapAreaChoice[] gateways;

    public int phase;

    private void OnEnable()
    {
        foreach (MapAreaChoice choice in gateways)
        {
            // When the map is loaded, set the choice trigger's to move to the corresponding node
            choice.gateway.OnPlayerEnteredGateway += () => MoveToNextArea(choice);
        }

        OnNextMapAreaChosen += node => { if (node == null) { Debug.LogError("StoryNode not set!"); } else { Debug.Log("Next StoryNode = " + node.name); } };
    }

    private void MoveToNextArea(MapAreaChoice mapAreaChoice)
    {
        foreach (var possibleMapArea in mapAreaChoice.possibleMapAreas)
        {
            if (possibleMapArea.conditions.All(c => c.Query()))
            {
                OnNextMapAreaChosen?.Invoke(possibleMapArea.resultingArea);
                return;
            }
        }

        mapAreaChoice.noMapConditionsMet?.Invoke();
    }

    [Serializable]
    public struct MapAreaChoice
    {
        public ChoiceGateway gateway; // The physical gateway itself
        public MapAreaCondition[] possibleMapAreas;
        public UnityEvent noMapConditionsMet;
    }

    [Serializable]
    public struct MapAreaCondition
    {
        public MapArea resultingArea; // The next map area to be loaded
        public QueryCondition_Base[] conditions;
    }
}
