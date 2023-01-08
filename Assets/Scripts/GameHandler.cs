using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static event Action OnMapAreaChanged;

    public static event Action<bool> OnNeutralModeChange;

    public static event Action<Vector3> OnGameOver;

    public static bool IsGameOver { get; private set; }

    public static bool IsPaused { get; private set; }

    public static MapArea CurrentMapArea { get; private set; }

    public static bool InNeutralMode { get; private set; }

    public static int CurrentPhase { get; private set; }

    [SerializeField] private MapArea startMapArea;

    private void Awake()
    {
        PlaythroughStats.Reset();
        Decisions.Reset();
        IsGameOver = false;
        IsPaused = false;
    }

    void Start()
    {
        MapArea.OnNextMapAreaChosen += UpdateScene;
        PlayerHealth.OnPlayerDie += OnPlayerDied;
        UpdateScene(startMapArea);
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

    private void UpdateScene (MapArea newArea)
    {
        if (CurrentMapArea != null)
        {
            Destroy(CurrentMapArea.gameObject, 0);
        }

        CurrentMapArea = Instantiate(newArea);
        CurrentPhase = newArea.phase;
        SetNeutralMode(newArea.NeutralArea);
        OnMapAreaChanged?.Invoke();
    }

    public static void SetPaused(bool pause)
    {
        IsPaused = pause;
        Time.timeScale = pause ? 0 : 1;
    }

    private void OnPlayerDied(Vector3 deathCausePosition)
    {
        IsGameOver = true;
        OnGameOver?.Invoke(deathCausePosition);
    }
}
