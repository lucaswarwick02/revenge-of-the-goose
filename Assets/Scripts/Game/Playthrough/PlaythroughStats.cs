using System;
using UnityEngine;

public static class PlaythroughStats
{
    public static void Reset()
    {
        DestructionScore = 0;
        EnemyKillCount = 0;
        AnimalKillCount = 0;
    }

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

    public static int EnemiesEncountered { get; private set; }

    public static void IncrementEnemiesEncountered() {
        EnemiesEncountered++;
    }


    // Event for when the animal kill count is incremented <new count, world position of kill event>
    public static event Action<int, Vector3> OnAnimalKilled;

    public static int AnimalKillCount { get; private set; }

    public static void IncrementAnimalKillCount(Vector3 atPosition)
    {
        AnimalKillCount++;
        OnAnimalKilled?.Invoke(AnimalKillCount, atPosition);
    }

    public static int AnimalsEncountered { get; private set; }

    public static void IncrementAnimalsEncountered () {
        AnimalsEncountered++;
    }

    // Percentage of Animals Killed
    public static float AnimalKillPercentage () {
        return (AnimalsEncountered == 0) ? 0 : (float) AnimalKillCount / (float) AnimalsEncountered;
    }

    // Functions for checking if companions are unlocked
    public static bool IsBunnyCompanionUnlocked { get; private set; }
    public static bool IsSheepCompanionUnlocked { get; private set; }

    public static void UnlockBunnyCompanion () { IsBunnyCompanionUnlocked = true; }
    public static void UnlockSheepCompanion () { IsSheepCompanionUnlocked = true; }

    public enum Statistic
    {
        DestructionScore,
        AnimalsKilled,
        AnimalsEncountered,
        EnemiesKilled,
        EnemiesEncountered,
        HasBunnyCompanion_Bool,
        HasSheepCompanion_Bool,
    }

    public enum Predicate
    {
        Equal,
        GreaterThan,
        LessThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        NotEqual,
    }

    public struct StatisticQuery
    {
        public Statistic variable;
        public Predicate predicate;
        public int value;
    }

    public static bool Query(StatisticQuery query)
    {
        int value = query.variable switch
        {
            Statistic.AnimalsKilled => AnimalKillCount,
            Statistic.EnemiesKilled => EnemyKillCount,
            Statistic.DestructionScore => DestructionScore,
            Statistic.AnimalsEncountered => AnimalsEncountered,
            Statistic.EnemiesEncountered => EnemiesEncountered,
            Statistic.HasBunnyCompanion_Bool => IsBunnyCompanionUnlocked ? 1 : 0,
            Statistic.HasSheepCompanion_Bool => IsSheepCompanionUnlocked ?  1 : 0,
            _ => throw new NotImplementedException(),
        };

        return query.predicate switch
        {
            Predicate.GreaterThan => value > query.value,
            Predicate.LessThan => value < query.value,
            Predicate.LessThanOrEqual => value <= query.value,
            Predicate.GreaterThanOrEqual => value >= query.value,
            Predicate.Equal => value == query.value,
            Predicate.NotEqual => value != query.value,
            _ => throw new NotImplementedException(),
        };
    }
}
