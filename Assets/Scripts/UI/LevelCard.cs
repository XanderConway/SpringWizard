using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
                // TODO find the fastest time
                TimeSpan timeSpan = TimeSpan.FromSeconds(data[0].totalTime);
                string formatTimer = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");

                scoreText.text = formatTimer;
                numCollected.text = data[0].numCollected;
                return;
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
