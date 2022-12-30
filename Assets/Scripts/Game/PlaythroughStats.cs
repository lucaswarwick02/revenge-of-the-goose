using System;
using UnityEngine;

public static class PlaythroughStats
{
    // Event for when the destruction score changes <new score, score change, world position of destruction event>
    public static event Action<int, int, Vector3> OnDestructionScoreChanged;

    public static int DestructionScore { get; private set; }

    public static void AddDestructionScore(int scoreGained, Vector3 atPosition)
    {
        DestructionScore += scoreGained;
        OnDestructionScoreChanged?.Invoke(DestructionScore, scoreGained, atPosition);
    }


    public static event Action<int, int, Vector3> OnEnemyKillCountChanged;

    public static int EnemyKillCount { get; private set; }

    public static void IncrementEnemyKillCount(Vector3 atPosition, int noKilled = 1)
    {
        EnemyKillCount += noKilled;
        OnEnemyKillCountChanged?.Invoke(EnemyKillCount, noKilled, atPosition);
    }


    public static event Action<int, int, Vector3> OnFriendlyKillCountChanged;

    public static int FriendlyKillCount { get; private set; }

    public static void IncrementFriendlyKillCount(Vector3 atPosition, int noKilled = 1)
    {
        FriendlyKillCount += noKilled;
        OnFriendlyKillCountChanged?.Invoke(FriendlyKillCount, noKilled, atPosition);
    }
}
