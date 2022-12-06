using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public MapArea startMapArea;

    // Start is called before the first frame update
    void Start()
    {
        StoryNode.OnCurrentStoryNodeChange += UpdateScene;
        Instantiate(startMapArea);
    }

    public void UpdateScene (StoryNode node)
    {

    }
}
