using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExampleUI : MonoBehaviour
{
    public TMP_Text promptTextField;
    public Button choiceButtonPrefab;
    public Transform buttonContainer;

    public StoryNode startNode;

    // Start is called before the first frame update
    void OnEnable()
    {
        StoryNode.OnCurrentStoryNodeChange += UpdateUI;
        StoryNode.CurrentStoryNode = startNode;
    }
    void OnDisable()
    {
        StoryNode.OnCurrentStoryNodeChange -= UpdateUI;
    }

    public void UpdateUI(StoryNode node)
    {
        if (node == null)
        {
            Debug.LogError("Node was null!");
            return;
        }

        promptTextField.text = node.Prompt;

        for (int choiceIndex = 0; choiceIndex < node.Choices.Length; choiceIndex++)
        {
            Button newButton = Instantiate(choiceButtonPrefab, buttonContainer);
            newButton.GetComponent<TMP_Text>().text = node.Choices[choiceIndex].Text;
            newButton.onClick.AddListener(() => ButtonPressed(choiceIndex));
        }
    }

    public void ButtonPressed(int choiceIndex)
    {
        Choice choiceChosen = StoryNode.CurrentStoryNode.Choices[choiceIndex];

        if (choiceChosen is BasicChoice bChoice)
        {
            StoryNode.CurrentStoryNode = bChoice.NextStoryNode;
        }
        else
        {
            SceneManager.LoadSceneAsync((choiceChosen as MinigameChoice).MinigameScene);
        }
    }

}
