using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EpilogueController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image newspaperSR;
    [SerializeField] private TMP_Text newspaperText;
    [SerializeField] private TMP_Text statsField;
    [TextArea][SerializeField] private string statsText;

    [Header("Changes")]
    [SerializeField] private Sprite normalNewspaper;
    [SerializeField] private Sprite sadistNewspaper;
    [SerializeField] private Sprite pacifistNewspaper;
    [TextArea][SerializeField] private string normalText;
    [TextArea][SerializeField] private string sadistText;
    [TextArea][SerializeField] private string pacifistText;

    private void Awake()
    {
        Time.timeScale = 1;

        string endingNum;
        if (GameEnder.EndingRealised == GameEnder.Ending.Sadist)
        {
            endingNum = "1st";
            newspaperSR.sprite = sadistNewspaper;
            newspaperText.text = normalText;
        }
        else if (GameEnder.EndingRealised == GameEnder.Ending.Pacifict)
        {
            endingNum = "2nd";
            newspaperSR.sprite = pacifistNewspaper;
            newspaperText.text = sadistText;
        }
        else
        {
            endingNum = "3rd";
            newspaperSR.sprite = normalNewspaper;
            newspaperText.text = pacifistText;
        }

        statsField.text = string.Format(statsText, 
            string.Format(CultureInfo.InvariantCulture, "{0:0,0}", PlaythroughStats.DestructionScore), 
            PlaythroughStats.EnemyKillCount,
            PlaythroughStats.TotalAnimalsKilled, 
            endingNum);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
