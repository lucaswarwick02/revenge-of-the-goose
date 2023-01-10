using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "QueryConditions_Composite", menuName = "Query Conditions Composite")]
public class QueryConditions_Composite : QueryCondition_Base
{
    [SerializeField] private ConditionsAnyTrue_Composite[] conditionsAnyTrue;

    public override bool Query()
    {
        return conditionsAnyTrue.Length < 1 || conditionsAnyTrue.Any(or => or.conditionsAllTrue.Length < 1 || or.conditionsAllTrue.All(c => c.Query()));
    }

    [Serializable]
    public struct ConditionsAnyTrue_Composite
    {
        public QueryConditions[] conditionsAllTrue;
    }
}
