using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITrickDetector : MonoBehaviour, TrickObserver
{
    [SerializeField] private PlayerSubject _player;

    private int _trickScore = 0;
    private String _trickName;

    //UI Elements
    [SerializeField] private Text _trickScoreText;
    [SerializeField] private Text _trickNameText;



    public void UpdateTrickObserver(PlayerTricks playerTricks)
    {
       TrickDisplay(playerTricks);
       UpdateUI();
    }

    //TODO: for now I will have the trick name and score here, but this should be moved to a separate class

    private void TrickDisplay(PlayerTricks playerTrick){
        switch(playerTrick)
        {
            case PlayerTricks.FrontFlip:
                _trickName = "Front Flip";
                _trickScore += 100;
                 UpdateUI();
                break;
            case PlayerTricks.BackFlip:
                _trickName = "Back Flip";
                _trickScore += 100;
                break;
            case PlayerTricks.NoHandsFrontFlip:
                _trickName = "No Hands Front Flip";
                _trickScore += 200;
                break;
            case PlayerTricks.NoHandsBackFlip:
                _trickName = "No Hands Back Flip";
                _trickScore += 200;
                break;
            case PlayerTricks.NoFeetFrontFlip:
                _trickName = "No Feet Front Flip";
                _trickScore += 200;
                break;
            case PlayerTricks.NoFeetBackFlip:
                _trickName = "No Feet Back Flip";
                _trickScore += 200;
                break;
            default:
                _trickName = "";
                _trickScore += 0;
                break;
        }
    }
    private IEnumerator UpdateUIForLimitedTime(float displayTime)
    {
        _trickNameText.text = _trickName;
        _trickScoreText.text = "Score: " + _trickScore;
        yield return new WaitForSeconds(displayTime);
        _trickNameText.text = "";
    }

    private void UpdateUI()
    {
        StartCoroutine(UpdateUIForLimitedTime(2.0f)); // Display trick name for 2 seconds
    }


    
    void OnEnable()
    {
        _player.AddObserver(this);
    }

    void OnDisable()
    {
        _player.RemoveObserver(this);
    }
}
