using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
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
        GameHandler.SetPaused(true);

        checkpointButton.SetActive(GameHandler.AnyCheckpointReached);
    }

    public void LoadLastCheckpoint()
    {
        GameHandler.SetPaused(false);
        gameHandler.LoadCheckpoint();
        gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        GameHandler.SetPaused(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
