using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PlaythroughStats
{
    public static void Reset()
    {
        DestructionScore = 0;
        EnemyKillCount = 0;
        animalsKilledInPhases[GameHandler.CurrentPhase] = 0;
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

    public static void IncrementAnimalKillCount(Vector3 atPosition)
    {
        animalsKilledInPhases[GameHandler.CurrentPhase]++;
        OnAnimalKilled?.Invoke(animalsKilledInPhases[GameHandler.CurrentPhase], atPosition);
    }

    public static void IncrementAnimalsEncountered () {
        animalsEncounteredInPhases[GameHandler.CurrentPhase]++;
    }

    // Percentage of Animals Killed
    public static float AnimalKillPercentage () {
        return (animalsEncounteredInPhases[GameHandler.CurrentPhase] == 0) ? 0 : (float) animalsKilledInPhases[GameHandler.CurrentPhase] / (float) animalsEncounteredInPhases[GameHandler.CurrentPhase];
    }

    public static Dictionary<int, int> animalsKilledInPhases = new Dictionary<int, int>{
        {1, 0},
        {2, 0},
        {3, 0}
    };

    public static Dictionary<int, int> animalsEncounteredInPhases = new Dictionary<int, int>{
        {1, 0},
        {2, 0},
        {3, 0}
    };

    public static int TotalAnimalsKilled { get{ return animalsKilledInPhases.Sum(x => x.Value); } }
    public static int TotalAnimalsEncountered { get{ return animalsEncounteredInPhases.Sum(x => x.Value); }}

    [Serializable]
    public enum Statistic
    {
        DestructionScore,
        Phase1AnimalsKilled,
        Phase2AnimalsKilled,
        Phase3AnimalsKilled,
        TotalAnimalsKilled,
        TotalAnimalsEncountered,
        Phase1AnimalsEncountered,
        Phase2AnimalsEncountered,
        Phase3AnimalsEncountered,
        EnemiesKilled,
        EnemiesEncountered,
        HasBunnyCompanion_Bool,
        HasSheepCompanion_Bool,
    }

    [Serializable]
    public enum Predicate
    {
        Equal,
        GreaterThan,
        LessThan,
        LessThanOrEqual,
        GreaterThanOrEqual,
        NotEqual,
    }

    [Serializable]
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
            Statistic.Phase1AnimalsKilled => animalsKilledInPhases[1],
            Statistic.Phase2AnimalsKilled => animalsKilledInPhases[2],
            Statistic.Phase3AnimalsKilled => animalsKilledInPhases[3],
            Statistic.TotalAnimalsKilled => TotalAnimalsKilled,
            Statistic.EnemiesKilled => EnemyKillCount,
            Statistic.DestructionScore => DestructionScore,
            Statistic.Phase1AnimalsEncountered => animalsEncounteredInPhases[1],
            Statistic.Phase2AnimalsEncountered => animalsEncounteredInPhases[2],
            Statistic.Phase3AnimalsEncountered => animalsEncounteredInPhases[3],
            Statistic.TotalAnimalsEncountered => TotalAnimalsEncountered,
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

    // Functions for checking if companions are unlocked
    public static bool IsBunnyCompanionUnlocked { get; private set; }
    public static bool IsSheepCompanionUnlocked { get; private set; }

    public static void UnlockBunnyCompanion () { IsBunnyCompanionUnlocked = true; }
    public static void UnlockSheepCompanion () { IsSheepCompanionUnlocked = true; }
}
