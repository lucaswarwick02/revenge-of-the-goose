using System;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static bool isPaused = false;

    public static event Action OnSceneUpdated;
    
    [SerializeField] private MapArea startMapArea;

    public MapArea CurrentMapArea { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MapArea.OnNextMapAreaChosen += UpdateScene;
        UpdateScene(startMapArea);
    }

    public void UpdateScene (MapArea newArea)
    {
        if (CurrentMapArea != null)
        {
            Destroy(CurrentMapArea.gameObject);
        }

        CurrentMapArea = Instantiate(newArea);
        OnSceneUpdated?.Invoke();
    }
}
