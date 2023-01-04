using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public static bool isPaused;

    public void Pause() {
        if (isPaused) return;

        gameObject.SetActive(true);

        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPause () {
        gameObject.SetActive(false);

        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Continue () {
        UnPause();
    }

    public void ExitToMainMenu () {
        UnPause();
        SceneManager.LoadScene("MainMenu");
    }
}
