using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private bool pauseImmediately;
    [SerializeField] private bool setToNeutralModeImmediately;

    public static Ending EndingRealised { get; private set; }

    public enum Ending
    {
        Normal,
        Sadist,
        Pacifict,
    }

    private static void LoadEpilogue()
    {
        SceneManager.LoadSceneAsync("Epilogue");
    }

    public void NormalEnding()
    {
        StartCoroutine(EndGameAfterDelay(Ending.Normal));
    }

    public void SadistEnding()
    {
        StartCoroutine(EndGameAfterDelay(Ending.Sadist));
    }

    public void PacifistEnding()
    {
        StartCoroutine(EndGameAfterDelay(Ending.Pacifict));
    }

    private IEnumerator EndGameAfterDelay(Ending ending)
    {
        FindObjectOfType<Canvas>().gameObject.SetActive(false);

        if (pauseImmediately)
        {
            Time.timeScale = 0;
        }

        if (setToNeutralModeImmediately)
        {
            GameHandler.SetNeutralMode(true);
        }

        yield return new WaitForSecondsRealtime(delay);
        EndingRealised = ending;
        LoadEpilogue();
    }
}
