using System.Collections.Generic;
using UnityEngine;

public static class Decisions
{
    public static void Reset()
    {
        ConversationResponses = new ();
    }

    public static Dictionary<string, List<string>> ConversationResponses { get; private set; } = new ();
}
