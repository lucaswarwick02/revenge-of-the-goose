using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public SpriteRenderer groundRenderer;

    // Start is called before the first frame update
    void Start()
    {
        StoryNode.OnCurrentStoryNodeChange += UpdateScene;
    }

    public void UpdateScene (StoryNode node) {
        groundRenderer.sprite = node.GroundSprite;
    }
}
