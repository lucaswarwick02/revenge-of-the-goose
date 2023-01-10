using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "QueryConditions", menuName = "Query Conditions")]
public class QueryConditions : ScriptableObject
{
    [SerializeField] private ConditionsAnyTrue[] conditionsAnyTrue;
    [SerializeField] private Decisions.ResponseQuery[] responsesRequired;

    public bool Query()
    {
        return conditionsAnyTrue.Length < 1 || conditionsAnyTrue.Any(or => or.conditionsAllTrue.Length < 1 || or.conditionsAllTrue.All(c => PlaythroughStats.Query(c)) && responsesRequired.All(r => Decisions.Query(r)));
    }

    [Serializable]
    public struct ConditionsAnyTrue
    {
        public PlaythroughStats.StatisticQuery[] conditionsAllTrue;
    }
}
