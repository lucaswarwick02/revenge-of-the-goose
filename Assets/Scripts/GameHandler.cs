using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static event Action<bool> OnMapAreaChanged;

    public static event Action<Vector3> OnGameOver;

    public static bool IsGameOver { get; private set; }

    public static bool IsPaused { get; private set; }

    public static MapArea CurrentMapArea { get; private set; }

    public static bool InNeutralArea { get; private set; }

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

    private void UpdateScene (MapArea newArea)
    {
        if (CurrentMapArea != null)
        {
            Destroy(CurrentMapArea.gameObject);
        }

        CurrentMapArea = Instantiate(newArea);
        InNeutralArea = newArea.NeutralArea;
        OnMapAreaChanged?.Invoke(InNeutralArea);
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
