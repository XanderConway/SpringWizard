using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LevelCard : MonoBehaviour
{

    public string levelId;

    [SerializeField] 
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI numCollected;

    // Start is called before the first frame update
    void Start()
    {
        LoadScore();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadScore()
    {

        scoreText.text = "??:??";
        if (LevelManager.Instance != null) {
            List<ScoreData> data = LevelManager.Instance.getScores(levelId);
            if (data != null && data.Count > 0)
            {
            ScoreData fastestScore = data[0];
            for (int i = 1; i < data.Count; i++)
            {
                if (data[i].totalTime < fastestScore.totalTime)
                {
                    fastestScore = data[i];
                }
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(fastestScore.totalTime);
            string formatTimer = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");

            scoreText.text = formatTimer;
            numCollected.text = fastestScore.numCollected;
            return;
            }
        }
        
    }
    public void OpenLeaderboard()
    {
        if (LevelManager.Instance != null)
        {
            LevelMetaData data = LevelManager.Instance.getMetaData(levelId);

            if (data != null)
            {
                LevelManager.Instance.currentLevel = data;
                SceneManager.LoadScene("Leaderboard");
            }
        }
    }
    public void LoadLevel(bool isPractice)
    {
        if (LevelManager.Instance != null)
        {
            LevelMetaData data = LevelManager.Instance.getMetaData(levelId);

            if (data != null)
            {
                LevelManager.Instance.currentIsPractice = isPractice;
                LevelManager.Instance.currentLevel = data;
                SceneManager.LoadScene(data.sceneName);
            }
        }
    }
}
