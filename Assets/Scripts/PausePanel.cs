using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Pause() {
        if (GameHandler.IsPaused) return;

        gameObject.SetActive(true);

        GameHandler.SetPaused(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void UnPause () {
        gameObject.SetActive(false);

        GameHandler.SetPaused(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Continue () {
        UnPause();
    }

    public void ExitToMainMenu ()
    {
        SceneManager.LoadScene("MainMenu");
        GameHandler.SetPaused(false);
    }
}
