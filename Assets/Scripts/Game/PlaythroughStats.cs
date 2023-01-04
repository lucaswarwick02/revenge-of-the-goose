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


    // Event for when the enemy kill count is incremented <new count, world position of kill event>
    public static event Action<int, Vector3> OnEnemyKilled;

    public static int EnemyKillCount { get; private set; }

    public static void IncrementEnemyKillCount(Vector3 atPosition)
    {
        EnemyKillCount++;
        OnEnemyKilled?.Invoke(EnemyKillCount, atPosition);
    }


    // Event for when the animal kill count is incremented <new count, world position of kill event>
    public static event Action<int, Vector3> OnAnimalKilled;

    public static int AnimalKillCount { get; private set; }

    public static void IncrementAnimalKillCount(Vector3 atPosition)
    {
        AnimalKillCount++;
        OnAnimalKilled?.Invoke(AnimalKillCount, atPosition);
    }
}
