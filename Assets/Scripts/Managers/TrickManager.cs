using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickManager : MonoBehaviour, TrickObserver
{   

    // This script deals with the universal calculation of trick scores combos and other trick related stuff

    // Start is called before the first frame update
    [SerializeField] private PlayerSubject player;

    // boolean for checking if the player is in a combo
    private bool _isInCombo = false;

    private int _comboCount  = 0;
    // a variable to handle tripple combo count
    private bool _tripleCombo = false;

    private PlayerTricks _prevTrick = PlayerTricks.None;

    // Start is called before the first frame update
    void Start()
    {  
        _isInCombo = false;
        _tripleCombo = false;
    }
    void OnEnable()
    {
        player.AddObserver(this);
    }

    void OnDisable()
    {
        player.RemoveObserver(this);
    }


    // Update is called once per frame
    void Update()
    {
        // This method can be used to handle any updates that need to occur each frame.
    }

    public int comboCount()
    {
        return _comboCount;
    }

    public bool tripleCombo
    {
        get { return _tripleCombo; }
    }

    public bool isInCombo
    {
        get { return _isInCombo; }
    }


    public void comboCount(PlayerTricks trick)
    {
        Debug.Log("Player trick: " + trick);
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
                Debug.Log("Player repeated the same trick, combo failed");
                _isInCombo = false;
                _tripleCombo = false;
                _comboCount = 0;
            }

            // Debug.Log("Player is in a combo");

            else if (_isInCombo)
            {
                _comboCount++;
                if (_comboCount % 3 == 0)
                {
                    _tripleCombo = true;
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
            // Debug.Log("Player is in a combo with count: " + _comboCount);
        }
        Debug.Log("Combo count: " + _comboCount);
    }



    public void UpdateTrickObserver(PlayerTricks trick)
    {
        comboCount(trick);   
    }
}
