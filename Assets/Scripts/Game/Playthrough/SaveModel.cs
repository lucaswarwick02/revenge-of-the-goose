using System.Collections.Generic;

public struct SaveModel
{
    #region General

    public MapArea CheckpointMapArea { get; set; }

    #endregion

    #region Stats

    public bool AnimalsTurned { get; set; }

    public int DestructionScore { get; set; }

    public int EnemyKillCount { get; set; }

    public int EnemiesEncountered { get; set; }

    public bool IsBunnyCompanionUnlocked { get; set; }

    public bool IsSheepCompanionUnlocked { get; set; }

    public Dictionary<int, int> AnimalsKilledInPhases { get; set; }

    public Dictionary<int, int> AnimalsEncounteredInPhases { get; set; }

    #endregion

    #region Decisions

    public Dictionary<string, List<string>> ConversationResponses { get; set; }

    #endregion

    public static SaveModel NewEmptySave()
    {
        SaveModel sm = new()
        {
            // Playthrough Stats default values
            AnimalsTurned = false,
            DestructionScore = 0,
            EnemyKillCount = 0,
            EnemiesEncountered = 0,
            IsSheepCompanionUnlocked = false,
            IsBunnyCompanionUnlocked = false,

            AnimalsKilledInPhases = new Dictionary<int, int>()
            {
                {1, 0},
                {2, 0},
                {3, 0}
            },


            AnimalsEncounteredInPhases = new Dictionary<int, int>()
            {
                {1, 0},
                {2, 0},
                {3, 0}
            },

            // Decisions default values
            ConversationResponses = new()
        };

        return sm;
    }
}
