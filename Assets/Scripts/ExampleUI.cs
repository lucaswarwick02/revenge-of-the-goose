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
        currentNode = startNode;
        UpdateUI();
    }

    public void UpdateUI () {
        prompt.text = currentNode.prompt;

        choice1.text = currentNode.choice1.text;
        choice2.text = currentNode.choice2.text;
    }

    public void Choice1 () {
        if (currentNode.choice1.linkedNode == null) return;

        currentNode = currentNode.choice1.linkedNode;
        UpdateUI();
    }

    public void Choice2 () {
        if (currentNode.choice2.linkedNode == null) return;

        currentNode = currentNode.choice1.linkedNode;
        UpdateUI();
    }

}
