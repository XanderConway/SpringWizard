using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UiScoreSystem : MonoBehaviour, TrickObserver
{
    [SerializeField] private PlayerSubject player;
    [SerializeField] private GameObject collectibesParent;

    private int _trickScore = 0;
    private String _trickName;

    //UI Elements
    [SerializeField] private TextMeshProUGUI trickScoreText;
    [SerializeField] private TextMeshProUGUI trickNameText;
    [SerializeField] private TextMeshProUGUI comboScoreText;


   private bool _isInCombo = false;
    private int _comboCount  = 0;
    // a variable to handle tripple combo count
    private bool _tripleCombo = false;
    private PlayerTricks _prevTrick = PlayerTricks.None;



    public void UpdateTrickObserver(PlayerTricks playerTricks)
    {
       TrickDisplay(playerTricks);
       comboCount(playerTricks);
       UpdateUI();
    }

    //TODO: for now we will have the trick name and score here, but this is better to be moved to a separate class

    private void TrickDisplay(PlayerTricks playerTrick){

        int trickValue = 0;
        switch(playerTrick)
        {
            case PlayerTricks.FrontFlip:
                _trickName = "Front Flip!";
                trickValue += 100;
                 UpdateUI();
                break;
            case PlayerTricks.BackFlip:
                _trickName = "Back Flip!";
                trickValue += 100;
                break;
            case PlayerTricks.NoHandsFrontFlip:
                _trickName = "No Hands Front Flip!";
                trickValue += 125;
                break;
            case PlayerTricks.NoHandsBackFlip:
                _trickName = "No Hands Back Flip!";
                trickValue += 125;
                break;
            case PlayerTricks.NoFeetFrontFlip:
                _trickName = "No Feet Front Flip!";
                trickValue += 125;
                break;
            case PlayerTricks.NoFeetBackFlip:
                _trickName = "No Feet Back Flip!";
                trickValue += 125;
                break;
            default:
                _trickName = "";
                trickValue += 0;
                break;
        }

        _trickScore += (int)(trickValue * (1 + _comboCount / 10.0f));
    }

    void comboCount(PlayerTricks trick)
    {
        if (trick == PlayerTricks.None)
        {
            _isInCombo = false;
            _tripleCombo = false;
            _comboCount = 0;
            // Debug.Log("Player is not in a combo");
        }
        else
        {
            if (_isInCombo && _prevTrick == trick)
            {
                _isInCombo = false;
                _tripleCombo = false;
                _comboCount = 0;
            }

            else if (_isInCombo)
            {
                _comboCount++;
                if (_comboCount % 3 == 0)
                {
                    _tripleCombo = true;
                    Debug.Log("Player is in a triple combo");
                }
                else
                {
                    _tripleCombo = false;
                }
            }
            else
            {
                _isInCombo = true;
                _comboCount = 0;
            }

            _prevTrick = trick;
        }
    }
    private IEnumerator UpdateUIForLimitedTime(float displayTime)
    {
        trickNameText.text = _trickName;
        trickScoreText.text = "Score: " + _trickScore;
        comboScoreText.text = "Combo: x" + _comboCount;

        yield return new WaitForSeconds(displayTime);
        trickNameText.text = "";
    }

    private void UpdateUI()
    {
        StartCoroutine(UpdateUIForLimitedTime(2.0f)); // Display trick name for 2 seconds
    }

    private void updateScoreCollected(CollectibleData collectibleData)
    {
        _trickScore += (int)(collectibleData.points * (1 + _comboCount / 10.0f));
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

            foreach (Collectible c in collectibles)
            {
                c.getEvent().AddListener(updateScoreCollected);
            }
        }
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("score", _trickScore);
        player.RemoveObserver(this);
    }
}
