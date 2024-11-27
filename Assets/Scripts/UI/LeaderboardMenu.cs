using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LeaderboardMenu : MonoBehaviour
{
    [SerializeField] private Text score1Text;  
    [SerializeField] private Text score2Text;
    [SerializeField] private Text score3Text;
    [SerializeField] private Text score4Text;
    [SerializeField] private Text score5Text;
    [SerializeField] private Button backToEndButton;
    [SerializeField] private Button mainButton;

    private List<Button> menuButtons;

    void Start()
    {
        if (LevelManager.Instance == null)
        {
            return;
        }

        DisplayScores(LevelManager.Instance.currentLevel.levelId);
        menuButtons = new List<Button> { backToEndButton, mainButton };
}

    void DisplayScores(string levelId)
    {
        Text[] scoreTexts = { score1Text, score2Text, score3Text, score4Text, score5Text };
        List<ScoreData> scores = LevelManager.Instance.getScores(levelId);

        if (scores != null && scores.Count > 0)
        {
            for (int i = 0; i < scoreTexts.Length; i++)
            {
                if (i < scores.Count)
                {
                    ScoreData score = scores[i];
                    scoreTexts[i].text = $"{score.totalTime:F2}s";
                }
                else
                {
                    scoreTexts[i].text = "---";
                }
            }
        }
        else
        {
            for (int i = 0; i < scoreTexts.Length; i++)
            {
                scoreTexts[i].text = "---";
            }
        }
    }

    public void BackToLevelSelect()
    {
        SceneManager.LoadScene(1); 
    }
}