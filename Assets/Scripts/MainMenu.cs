using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject choicesPanel;

    public void Exit () {
        Application.Quit();
    }

    public void Play () {
        mainMenuPanel.SetActive(false); // Hide the Main Menu
        choicesPanel.SetActive(true); // Show the Choices Panel
    }
}
