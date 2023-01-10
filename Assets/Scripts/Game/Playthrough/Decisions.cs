using System;
using System.Collections.Generic;
using UnityEngine;

public static class Decisions
{
    public static void ResetDecisions()
    {
        Load(SaveModel.NewEmptySave());
    }

    public static void Load(SaveModel saveModel)
    {
        conversationResponses = saveModel.ConversationResponses;
    }

    public static void SaveToModel(ref SaveModel saveModel)
    {
        saveModel.ConversationResponses = new Dictionary<string, List<string>>(conversationResponses);
    }

    private static Dictionary<string, List<string>> conversationResponses = new ();

    private static bool ConversationWasStarted(string conversationID) => conversationResponses.ContainsKey(conversationID);

    private static bool PlayerResponded(string conversationID, string responseID) => conversationResponses.TryGetValue(conversationID, out List<string> responses) && responses.Contains(responseID);

    public static bool Query(ResponseQuery query)
    {
        return query.conversationID == string.Empty
            || (query.responseID == string.Empty && ConversationWasStarted(query.conversationID))
            || PlayerResponded(query.conversationID, query.responseID);
    }

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

    [Serializable]
    public struct ResponseQuery
    {
        public string conversationID;
        public string responseID;
    }
}
