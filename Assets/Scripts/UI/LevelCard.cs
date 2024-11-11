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
        if (LevelManager.Instance != null) {
            ScoreData data = LevelManager.Instance.getScores(levelId);
            if (data != null)
            {
                scoreText.text = data.score.ToString();
                numCollected.text = data.numCollected;
                return;
            }
        }
        scoreText.text = "000000";
        
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
