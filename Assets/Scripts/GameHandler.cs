using System;
using System.Collections;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public const float NEW_AREA_SWITCH_TIME = 1;

    public static event Action OnMapAreaStartChanging;

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
        PlayerHealth.ResetHealth();

        UpdateScene(startMapArea, () => OnGameLoaded?.Invoke(), false);
    }

    public void LoadCheckpoint()
    {
        if (LastCheckpoint.HasValue)
        {
            IsGameOver = false;
            IsPaused = false;
            PlaythroughStats.Load(LastCheckpoint.Value);
            Decisions.Load(LastCheckpoint.Value);

            UpdateScene(LastCheckpoint.Value.CheckpointMapArea, () => OnGameLoaded?.Invoke());
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
        StartCoroutine(UpdateSceneRoutine(newArea, null));
    }

    private void UpdateScene(MapArea newArea, Action callback, bool useSwitchTime = true)
    {
        StartCoroutine(UpdateSceneRoutine(newArea, callback, useSwitchTime));
    }

    private IEnumerator UpdateSceneRoutine(MapArea newArea, Action callback, bool useSwitchTime = true)
    {
        Time.timeScale = 0;

        if (useSwitchTime)
        {
            OnMapAreaStartChanging?.Invoke();
        }

        yield return new WaitForSecondsRealtime(useSwitchTime? NEW_AREA_SWITCH_TIME : 0);

        if (CurrentMapAreaInstance != null)
        {
            foreach (ParticleSystem ps in FindObjectsOfType<ParticleSystem>())
            {
                ps.Clear();
            }

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

        Time.timeScale = 1;
        OnMapAreaChanged?.Invoke();
        callback?.Invoke();
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
