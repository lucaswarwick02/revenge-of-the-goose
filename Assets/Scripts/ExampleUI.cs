using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExampleUI : MonoBehaviour
{
    public TextMeshProUGUI prompt;
    public TextMeshProUGUI choice1;
    public TextMeshProUGUI choice2;

    public StoryNode startNode;
    StoryNode currentNode;

    // Start is called before the first frame update
    void Start()
    {
        StoryNode.OnNodeChange.AddListener(UpdateUI);
        StoryNode.OnNodeChange.Invoke(startNode);
    }

    public void UpdateUI (StoryNode node) {
        this.currentNode = node;
        prompt.text = node.prompt;

        choice1.text = node.choice1.text;
        choice2.text = node.choice2.text;
    }

    public void Choice1 () {
        if (currentNode.choice1.linkedNode == null) return;

        StoryNode.OnNodeChange.Invoke(currentNode.choice1.linkedNode);
    }

    public void Choice2 () {
        if (currentNode.choice2.linkedNode == null) return;

        StoryNode.OnNodeChange.Invoke(currentNode.choice2.linkedNode);
    }

}
