using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UiScoreSystem : MonoBehaviour, TrickObserver
{
    [SerializeField] private PlayerSubject player;
    [SerializeField] private GameObject collectibesParent;

    private int _totalScore = 0;
    private int _trickScore = 0;


    private String _trickName;

    //UI Elements
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI collectedDisplayText;

    [SerializeField] private TextMeshProUGUI trickNameText;
    [SerializeField] private TextMeshProUGUI trickScoreText;

    [SerializeField] private TextMeshProUGUI comboScoreText;

   
    private int totalCollectables = 0;
    private int numCollected = 0;


    //combo system
    private List<PlayerTricks> _tricksInCombo = new List<PlayerTricks>();
    private bool _isInCombo = false;
    


    //hashmap of tricks to score and name
    private Dictionary<PlayerTricks, (int score, string name)> trickScores = new Dictionary<PlayerTricks, (int score, string name)>
    {
        {PlayerTricks.None, (0, "")},
        {PlayerTricks.Death, (0, "")},
        {PlayerTricks.FrontFlip, (100, "Front Flip")},
        {PlayerTricks.BackFlip, (100, "Back Flip")},
        {PlayerTricks.NoHandsFrontFlip, (125, "No Hands Front Flip")},
        {PlayerTricks.NoHandsBackFlip, (125, "No Hands Back Flip")},
        {PlayerTricks.NoFeetFrontFlip, (125, "No Feet Front Flip")},
        {PlayerTricks.NoFeetBackFlip, (125, "No Feet Back Flip")}
    };

    public void UpdateTrickObserver(PlayerTricks playerTricks)
    {
       TrickDisplay(playerTricks);
       UpdateUI();
    }

    //TODO: for now I will have the trick name and score here, but this should be moved to a separate class

    private void TrickDisplay(PlayerTricks playerTrick){
        
        comboCount(playerTrick);

        if(_tricksInCombo.Count <= 1){
            _trickName = trickScores[playerTrick].name;
        }
        else{
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
        

        int updateToTotal;
        
        if (_tricksInCombo.Count > 1){
            updateToTotal = _trickScore * _tricksInCombo.Count;
        }
        else{
            updateToTotal = trickScores[playerTrick].score;
        }
        _totalScore += updateToTotal;

    }

    private string trickScoreDisplay(bool math){
        if (math && _tricksInCombo.Count > 1 ){
            return "" + _trickScore + "x" + _tricksInCombo.Count;
        }
        else if(_trickScore != 0){
            int curr_total = _trickScore * _tricksInCombo.Count;
            return "" + curr_total;
        }
        else{
            return "";
        }
    }
    
    void comboCount(PlayerTricks trick)
    {
        if (trick == PlayerTricks.None || trick == PlayerTricks.Death)
        {
            _isInCombo = false;
            _tricksInCombo.Clear();
            _trickScore = 0;
        }
        else
        {   
        
            if (_isInCombo && _tricksInCombo[_tricksInCombo.Count - 1] == trick)
            {
                _isInCombo = false;
                _tricksInCombo.Clear();
                _tricksInCombo.Add(trick);
                _trickScore = trickScores[trick].score;
            }
            else{
                _isInCombo = true;
                _tricksInCombo.Add(trick);
                _trickScore += trickScores[trick].score;

            }
           
        }
    }

    private IEnumerator UpdateWithTime()
    {   
        trickNameText.text = _trickName;
        trickScoreText.text = trickScoreDisplay(true);
        yield return new WaitForSeconds(0.5f);
        trickScoreText.text = trickScoreDisplay(false);
        yield return new WaitForSeconds(1.0f);
        trickScoreText.text = "";
        trickNameText.text = "";
    }

    private void UpdateUI()
    {
        totalScoreText.text = "Score: " + _totalScore;
        comboScoreText.text = "Combo: x" + _tricksInCombo.Count;
        StartCoroutine(UpdateWithTime());
    }

    private void updateScoreCollected(CollectibleData collectibleData)
    {
        _totalScore += (int)(collectibleData.points * (1 + _tricksInCombo.Count / 10.0f));
        numCollected += 1;
        collectedDisplayText.text = $"Scrolls: {numCollected} / {totalCollectables}";
        UpdateUI();
        Debug.Log("Collected!");
    }

    void OnEnable()
    {
        player.AddObserver(this);
    }

    void Start()
    {
        // Add as observer to collectibles

        if(collectibesParent != null)
        {
            Collectible[] collectibles = collectibesParent.GetComponentsInChildren<Collectible>();

            totalCollectables = collectibles.Length;

            foreach (Collectible c in collectibles)
            {
                c.getEvent().AddListener(updateScoreCollected);
            }
        }

        collectedDisplayText.text = $"Scrolls: {numCollected} / {totalCollectables}";
    }

    void OnDisable()
    {
        // Todo store score in LevelManager
        PlayerPrefs.SetInt("score", _totalScore);
        PlayerPrefs.SetString("numCollected", $"{numCollected} / {totalCollectables}");
        player.RemoveObserver(this);
    }
}
