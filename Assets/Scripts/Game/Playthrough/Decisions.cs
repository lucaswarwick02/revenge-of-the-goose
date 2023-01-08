using System.Collections.Generic;
using UnityEngine;

public static class Decisions
{
    public static void Reset()
    {
        conversationResponses = new ();
    }

    private static Dictionary<string, List<string>> conversationResponses = new ();

    public static bool ConversationWasStarted(string conversationID) => conversationResponses.ContainsKey(conversationID);

    public static bool PlayerResponded(string conversationID, string responseID) => conversationResponses.TryGetValue(conversationID, out List<string> responses) && responses.Contains(responseID);

    public static void RecordConversationStarted(string conversationID)
    {
        // Add new empty entry
        conversationResponses.Add(conversationID, new List<string>());
    }

    public static void RecordPlayerResponse(string conversationID, string responseID)
    {
        // Check if conversation already has an entry (meaning it has started)
        if (conversationResponses.TryGetValue(conversationID, out List<string> responses))
        {
            // Get list of responses and add new response
            responses.Add(responseID);
            conversationResponses[conversationID] = responses;
        }
        else
        {
            Debug.LogError("Conversation has not been recorded to have been started.");
        }
    }
}
