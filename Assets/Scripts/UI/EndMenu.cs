using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EndMenu : MonoBehaviour
{   
    [SerializeField] private Text _trickScoreText;
    [SerializeField] private Button BackToMainButton;
    [SerializeField] private Button quitButton;

    private int playerScore;

    private PlayerInputActions playerInputActions;
    private List<Button> menuButtons;
    private int selectedButtonIndex = 0; // Track the currently selected button

    private float buttonPressCooldown = 0.5f; // Cooldown time in seconds
    private float lastButtonPressTime = 0f;   // Time the last button was pressed


    void Start()
    {
        playerScore = PlayerPrefs.GetInt("score");
        string numCollected = PlayerPrefs.GetString("numCollected");
        _trickScoreText.text =  "" + playerScore;

        // Initialize the list of buttons
        menuButtons = new List<Button> { BackToMainButton, quitButton };

        if(LevelManager.Instance != null && LevelManager.Instance.currentLevel != null)
        {
            ScoreData scoreData = new ScoreData(numCollected, playerScore);

            LevelManager.Instance.scoreMap[LevelManager.Instance.currentLevel.levelId] = scoreData;
            LevelManager.Instance.saveScores();
        }
    }

    // Button actions
    public void BackToMain()
    {
        //load scene 0
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
