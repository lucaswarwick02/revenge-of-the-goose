using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnder : MonoBehaviour
{
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
        EndingRealised = Ending.Normal;
        LoadEpilogue();
    }

    public void SadistEnding()
    {
        EndingRealised = Ending.Sadist;
        LoadEpilogue();
    }

    public void PacifistEnding()
    {
        EndingRealised = Ending.Pacifict;
        LoadEpilogue();
    }
}
