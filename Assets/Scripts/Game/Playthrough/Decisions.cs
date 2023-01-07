using System.Collections.Generic;
using UnityEngine;

public static class Decisions
{
    public static void Reset()
    {
        conversationResponses = new ();
    }

    private static Dictionary<string, List<string>> conversationResponses = new ();

    public static bool PlayerResponded(string conversationID, string responseID) => conversationResponses.TryGetValue(conversationID, out List<string> responses) && responses.Contains(responseID);

    public static void RecordPlayerResponse(string conversationID, string responseID)
    {
        // Check if conversation already has an entry
        if (conversationResponses.TryGetValue(conversationID, out List<string> responses))
        {
            // Get list of responses and add new response
            responses.Add(responseID);
            conversationResponses[conversationID] = responses;
        }
        else
        {
            // Add new entry
            conversationResponses.Add(conversationID, new List<string>() { responseID });
        }
    }
}
