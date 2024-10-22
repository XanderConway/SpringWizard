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
    [SerializeField] private PlayerSubject _player;
    [SerializeField] private GameObject _collectibesParent;

    private int _trickScore = 0;
    private String _trickName;

    //UI Elements
    [SerializeField] private TextMeshProUGUI _trickScoreText;
    [SerializeField] private TextMeshProUGUI _trickNameText;
    [SerializeField] private TextMeshProUGUI _comboScoreText;

    private float combo = 0;
    private PlayerTricks _prevTrick = PlayerTricks.None;



    public void UpdateTrickObserver(PlayerTricks playerTricks)
    {
       TrickDisplay(playerTricks);
       UpdateUI();
    }

    //TODO: for now I will have the trick name and score here, but this should be moved to a separate class

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
                trickValue += 200;
                break;
            case PlayerTricks.NoHandsBackFlip:
                _trickName = "No Hands Back Flip!";
                trickValue += 200;
                break;
            case PlayerTricks.NoFeetFrontFlip:
                _trickName = "No Feet Front Flip!";
                trickValue += 200;
                break;
            case PlayerTricks.NoFeetBackFlip:
                _trickName = "No Feet Back Flip!";
                trickValue += 200;
                break;
            default:
                _trickName = "";
                trickValue += 0;
                break;
        }

        _trickScore += (int)(trickValue * (1 + combo / 10.0f));

        if(playerTrick != _prevTrick)
        {
            combo += 1;
            _prevTrick = playerTrick;
        }

        if(playerTrick == PlayerTricks.Death)
        {
            combo = 0;
        }
    }
    private IEnumerator UpdateUIForLimitedTime(float displayTime)
    {
        _trickNameText.text = _trickName;
        _trickScoreText.text = "Score: " + _trickScore;
        _comboScoreText.text = "Combo: x" + combo;

        yield return new WaitForSeconds(displayTime);
        _trickNameText.text = "";
    }

    private void UpdateUI()
    {
        StartCoroutine(UpdateUIForLimitedTime(2.0f)); // Display trick name for 2 seconds
    }

    private void updateScoreCollected(CollectibleData collectibleData)
    {
        _trickScore += collectibleData.points;
        UpdateUI();
        Debug.Log("Collected!");
    }

    void OnEnable()
    {
        _player.AddObserver(this);
    }

    void Start()
    {
        // Add as observer to collectibles

        if(_collectibesParent != null)
        {
            Collectible[] collectibles = _collectibesParent.GetComponentsInChildren<Collectible>();

            foreach (Collectible c in collectibles)
            {
                c.getEvent().AddListener(updateScoreCollected);
            }
        }
    }

    void OnDisable()
    {
        _player.RemoveObserver(this);
    }
}
