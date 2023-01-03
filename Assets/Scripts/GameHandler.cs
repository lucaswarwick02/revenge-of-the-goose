using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private MapArea startMapArea;

    public MapArea CurrentMapArea { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MapArea.OnNextMapAreaChosen += UpdateScene;
        CurrentMapArea = Instantiate(startMapArea);
    }

    public void UpdateScene (MapArea newArea)
    {
        Destroy(CurrentMapArea.gameObject);
        CurrentMapArea = Instantiate(newArea);
    }
}
