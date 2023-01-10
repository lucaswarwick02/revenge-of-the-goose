using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
    [SerializeField] private float delay;

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
        yield return new WaitForSecondsRealtime(delay);
        EndingRealised = ending;
        LoadEpilogue();
    }
}
