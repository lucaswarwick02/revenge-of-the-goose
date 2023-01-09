using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static event Action OnMapAreaChanged;

    public static event Action<bool> OnNeutralModeChange;

    public static event Action<Transform> OnGameOver;

    public static event Action OnGameLoaded;

    public static event Action OnGameSaved;

    public static bool IsGameOver { get; private set; }

    public static bool IsPaused { get; private set; }

    public static MapArea CurrentMapArea { get; private set; }

    public static MapArea CurrentMapAreaInstance { get; private set; }

    public static bool InNeutralMode { get; private set; }

    public static int CurrentPhase { get; private set; }

    public static bool AnyCheckpointReached => LastCheckpoint.HasValue;

    private static SaveModel? LastCheckpoint;

    [SerializeField] private MapArea startMapArea;

    private void Awake()
    {
        MapArea.OnNextMapAreaChosen += UpdateScene;
        PlayerHealth.OnPlayerDie += OnPlayerDied;
    }

    void Start()
    {
        StartNewGame();
    }

    private void OnDisable()
    {
        MapArea.OnNextMapAreaChosen -= UpdateScene;
        PlayerHealth.OnPlayerDie -= OnPlayerDied;
    }

    public static void SetNeutralMode(bool isNeutral)
    {
        if (InNeutralMode != isNeutral)
        {
            InNeutralMode = isNeutral;
            OnNeutralModeChange?.Invoke(isNeutral);
        }
    }

    public void StartNewGame()
    {
        LastCheckpoint = null;
        IsGameOver = false;
        IsPaused = false;
        PlaythroughStats.ResetPlaythrough();
        Decisions.ResetDecisions();

        UpdateScene(startMapArea);

        Debug.Log($"New game successfully started at {CurrentMapArea.name}");
        OnGameLoaded?.Invoke();
    }

    public void LoadCheckpoint()
    {
        if (LastCheckpoint.HasValue)
        {
            IsGameOver = false;
            IsPaused = false;
            PlaythroughStats.Load(LastCheckpoint.Value);
            Decisions.Load(LastCheckpoint.Value);

            UpdateScene(LastCheckpoint.Value.CheckpointMapArea);

            Debug.Log($"Checkpoint successfully loaded at {CurrentMapArea.name}");
            OnGameLoaded?.Invoke();
        }
        else
        {
            Debug.LogError("Could not load checkpoint as one was never made. Starting new game instead.");
            StartNewGame();
        }
    }

    private void SaveGame()
    {
        SaveModel newSave = SaveModel.NewEmptySave();
        newSave.CheckpointMapArea = CurrentMapArea;
        PlaythroughStats.SaveToModel(ref newSave);
        Decisions.SaveToModel(ref newSave);
        LastCheckpoint = newSave;

        Debug.Log($"Game successfully saved at {CurrentMapArea.name}");
        OnGameSaved?.Invoke();
    }

    private void UpdateScene(MapArea newArea)
    {
        if (CurrentMapAreaInstance != null)
        {
            Destroy(CurrentMapAreaInstance.gameObject, 0);
        }

        CurrentMapArea = newArea;
        CurrentMapAreaInstance = Instantiate(newArea);
        CurrentPhase = newArea.phase;
        SetNeutralMode(newArea.NeutralArea);

        if (newArea.IsCheckpoint)
        {
            SaveGame();
        }

        OnMapAreaChanged?.Invoke();
    }

    public static void SetPaused(bool pause)
    {
        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause;
        IsPaused = pause;
        Time.timeScale = pause ? 0 : 1;
    }

    private void OnPlayerDied(Transform killer)
    {
        IsGameOver = true;
        OnGameOver?.Invoke(killer);
    }

    public static void LoseBossFight (Transform player) {
        IsGameOver = true;
        OnGameOver?.Invoke(player);
    }
}
