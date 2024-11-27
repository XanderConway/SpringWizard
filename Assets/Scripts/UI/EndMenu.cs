using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class EndMenu : MonoBehaviour
{   
    [SerializeField] private Text trickScoreText;
    [SerializeField] private TextMeshProUGUI numScrollsText;
    [SerializeField] private Button BackToMainButton;
    [SerializeField] private Button leaderboardButton;
    [SerializeField] private Button quitButton;

    private int playerScore;

    private PlayerInputActions playerInputActions;
    private List<Button> menuButtons;
    private int selectedButtonIndex = 0; // Track the currently selected button

    private float buttonPressCooldown = 0.5f; // Cooldown time in seconds
    private float lastButtonPressTime = 0f;   // Time the last button was pressed

    private string GetFeedback(string input)
    {
        string[] parts = input.Split('/');
        if (parts.Length != 2 || !int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y) || y == 0)
        {
            return "";
        }

        // Calculate the ratio
        double ratio = (double)x / y;

        // Determine the feedback based on the ratio
        if (ratio == 1.0)
        {
            return "PERFECT!";
        }
        else if (ratio >= 0.7)
        {
            return "So close!";
        }
        else if (ratio >= 0.5)
        {
            return "Nice Try!";
        }
        else
        {
            return "Keep Practicing!";
        }
    }

    void Start()
    {
        playerScore = PlayerPrefs.GetInt("score");
        string numCollected = PlayerPrefs.GetString("numCollected");
        trickScoreText.text =  "" + playerScore;

        numScrollsText.text = GetFeedback(numCollected) + $" You Collected {numCollected} Scrolls!";

        // Initialize the list of buttons
        menuButtons = new List<Button> { BackToMainButton, leaderboardButton, quitButton };
    }

    // Button actions
    public void BackToMain()
    {
        //load scene 0
        SceneManager.LoadScene(0);
    }

    public void Leaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}