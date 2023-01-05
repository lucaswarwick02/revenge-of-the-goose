using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public void Pause() {
        if (GameHandler.isPaused) return;

        gameObject.SetActive(true);

        GameHandler.isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPause () {
        gameObject.SetActive(false);

        GameHandler.isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Continue () {
        UnPause();
    }

    public void ExitToMainMenu () {
        GameHandler.isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}
