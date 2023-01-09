using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject checkpointButton;

    private GameHandler gameHandler;

    private void Awake()
    {
        gameHandler = FindObjectOfType<GameHandler>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        checkpointButton.SetActive(GameHandler.AnyCheckpointReached);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnPause();
        }
    }

    public void Pause() {
        if (GameHandler.IsPaused) return;
        gameObject.SetActive(true);
        GameHandler.SetPaused(true);
    }

    public void UnPause () {
        gameObject.SetActive(false);
        GameHandler.SetPaused(false);
    }

    public void LoadLastCheckpoint()
    {
        gameHandler.LoadCheckpoint();
        GameHandler.SetPaused(false);
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
