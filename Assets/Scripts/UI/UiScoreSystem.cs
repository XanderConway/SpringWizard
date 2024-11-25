using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UiScoreSystem : MonoBehaviour, TrickObserver
{
    [SerializeField] private TrickSubject player;
    [SerializeField] private UITimer timer;
    [SerializeField] private GameObject collectibesParent;

    private int totalScore = 0;
    private int _trickScore = 0;


    private String _trickName;

    //UI Elements
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI collectedDisplayText;

    [SerializeField] private TextMeshProUGUI trickNameText;
    [SerializeField] private TextMeshProUGUI trickScoreText;
    [SerializeField] private Image scoreFill;
    [SerializeField] private Image scrollFill;

    private int totalCollectables = 0;
    private int numCollected = 0;
    private int scoreRequirement = 5000;


    //combo system
    private List<PlayerTricks> _tricksInCombo = new List<PlayerTricks>();
    private bool _isInCombo = false;
    private int _validComboCount = 0;



    //hashmap of tricks to score and name
    private Dictionary<PlayerTricks, (int score, string name)> trickScores = new Dictionary<PlayerTricks, (int score, string name)>
    {
        {PlayerTricks.None, (0, "")},
        {PlayerTricks.Death, (0, "")},
        {PlayerTricks.FrontFlip, (100, "Front Flip")},
        {PlayerTricks.BackFlip, (100, "Back Flip")},
        {PlayerTricks.NoHands, (50, "No Handed")},
        {PlayerTricks.Kickflip, (50, "Kick Flip")},
        {PlayerTricks.ScissorKick, (50, "Scissor Kick")},
        {PlayerTricks.HandlessBarSpin, (50, "Handless Bar Spin")},
        {PlayerTricks.springboard, (200, "Springboard")},
        {PlayerTricks.RailGrinding, (200, "Rail Grinding")},
        {PlayerTricks.WallJump, (75, "Wall Jump")}
    };

    public void UpdateTrickObserver(PlayerTricks playerTricks)
    {
        TrickDisplay(playerTricks);

        bool comboEnded = playerTricks == PlayerTricks.None || playerTricks == PlayerTricks.Death;
        UpdateUI(comboEnded);
    }

    //TODO: for now I will have the trick name and score here, but this should be moved to a separate class

    private void TrickDisplay(PlayerTricks playerTrick)
    {

        comboCount(playerTrick);

        if (_validComboCount <= 1)
        {
            _trickName = trickScores[playerTrick].name;
        }
        else
        {
            _trickName = "";
            for (int i = 0; i < _tricksInCombo.Count; i++)
            {
                _trickName += trickScores[_tricksInCombo[i]].name;
                if (i < _tricksInCombo.Count - 1)
                {
                    _trickName += " + ";
                }
            }
        }

    }

    private string trickScoreDisplay(bool math)
    {
        if (math && _validComboCount > 1)
        {
            return "" + _trickScore + "x" + _validComboCount;
        }
        else if (_trickScore != 0)
        {
            int curr_total = _trickScore * _validComboCount;
            return "" + curr_total;
        }
        else
        {
            return "";
        }
    }

    void comboCount(PlayerTricks trick)
    {
        if (trick == PlayerTricks.None || trick == PlayerTricks.Death)
        {

            _isInCombo = false;
            _tricksInCombo.Clear();
            totalScore += _trickScore * _validComboCount;
            _trickScore = 0;
            _validComboCount = 0;
        }
        else
        {
            
            _isInCombo = true;
            if (!_tricksInCombo.Contains(trick))
            {
                _validComboCount += 1;
                _tricksInCombo.Add(trick);
            }
            _trickScore += trickScores[trick].score;
        }
    }

    private IEnumerator UpdateWithTime()
    {
        //trickNameText.text = _trickName;
        //trickScoreText.text = trickScoreDisplay(true);
        //yield return new WaitForSeconds(1.0f);
        //trickScoreText.text = trickScoreDisplay(false);
        //trickNameText.text = _trickName;
        yield return new WaitForSeconds(0.5f);
        trickScoreText.text = "";
        trickNameText.text = "";
    }

    private void UpdateUI(bool comboEnded)
    {
        totalScoreText.text = $"Score {totalScore} / {scoreRequirement}";

        scoreFill.fillAmount = Mathf.Clamp((float)(totalScore) / scoreRequirement, 0f, 1f);

        scrollFill.fillAmount = Mathf.Clamp((float)(numCollected) / totalCollectables, 0f, 1f);

        if (!comboEnded)
        {
            trickNameText.text = _trickName;
            trickScoreText.text = trickScoreDisplay(true);
        } else
        {
            trickScoreText.text = trickScoreDisplay(false);
            StartCoroutine(UpdateWithTime());
        }
    }

    private void updateScoreCollected(CollectibleData collectibleData)
    {
        totalScore += (int)(collectibleData.points * (1 + _validComboCount / 10.0f));

        numCollected += 1;
        collectedDisplayText.text = $"Scrolls {numCollected} / {totalCollectables}";
        UpdateUI(false);
        Debug.Log("Collected!");
    }

    void OnEnable()
    {
        player.AddObserver(this);
    }

    void Start()
    {
        // Add as observer to collectibles

        if (collectibesParent != null)
        {
            Collectible[] collectibles = collectibesParent.GetComponentsInChildren<Collectible>();

            totalCollectables = collectibles.Length;

            foreach (Collectible c in collectibles)
            {
                c.getEvent().AddListener(updateScoreCollected);
            }
        }



        if (LevelManager.Instance != null && LevelManager.Instance.currentLevel != null)
        {
            scoreRequirement = LevelManager.Instance.currentLevel.scoreRequirement;
        }

        collectedDisplayText.text = $"Scrolls {numCollected} / {totalCollectables}";
    }

    void Update()
    {
        checkFinished();
    }

    void checkFinished()
    {
        if (numCollected == totalCollectables && totalScore >= scoreRequirement)
        {

            // Save the score
            if (LevelManager.Instance != null && LevelManager.Instance.currentLevel != null)
            {
                ScoreData data = new ScoreData(numCollected.ToString(), totalScore, timer.currentTime);
                string levelId = LevelManager.Instance.currentLevel.levelId;
                LevelManager.Instance.saveScore(data, levelId);
            }

            SceneManager.LoadScene("EndMenu");
        }
    }

    void OnDisable()
    {
        // Todo store score in LevelManager
        PlayerPrefs.SetInt("score", totalScore);
        PlayerPrefs.SetString("numCollected", $"{numCollected} / {totalCollectables}");
        player.RemoveObserver(this);
    }
}
