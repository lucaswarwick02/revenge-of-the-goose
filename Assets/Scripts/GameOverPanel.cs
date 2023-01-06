using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameHandler.SetPaused(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
