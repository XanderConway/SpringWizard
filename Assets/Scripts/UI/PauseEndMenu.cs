using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PauseEndMenu : MonoBehaviour
{   
    [SerializeField] private UiScoreSystem TrickDetector;
    [SerializeField] private Text trickScoreText;
    [SerializeField] private TextMeshProUGUI timeUseText;
    [SerializeField] private Button keepPlayingButton;
    [SerializeField] private Button finishButton;
    [SerializeField] private Button restartButton;

    private int playerScore;

    private PlayerInputActions playerInputActions;
    private List<Button> menuButtons;
    private int selectedButtonIndex = 0; // Track the currently selected button

    private float buttonPressCooldown = 0.5f; // Cooldown time in seconds
    private float lastButtonPressTime = 0f;   // Time the last button was pressed
    

    private string GetFeedback(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return "";
        }

        string[] timeParts = input.Split(':');
        if (timeParts.Length != 2 || !int.TryParse(timeParts[0], out int minutes) || !int.TryParse(timeParts[1], out int seconds))
        {
            return "";
        }

        float timeSpent = minutes * 60 + seconds;

        // Determine the feedback based on the time spent
        if (timeSpent <= 30.0f)
        {
            return "Amazing Speed!";
        }
        else if (timeSpent <= 60.0f)
        {
            return "Great Job!";
        }
        else if (timeSpent <= 120.0f)
        {
            return "Good Effort!";
        }
        else
        {
            return "Keep Practicing!";
        }
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        playerScore = TrickDetector.getTotalScore();
        string time = TrickDetector.getFormatTime();
        
        Debug.Log("Player Score: " + playerScore);
        Debug.Log("Time: " + time);
        
        trickScoreText.text =  "Your Score: " + playerScore;
        if (timeUseText != null){

            if (time == "00:00" || time == "0:00")
            {
                timeUseText.text = "You completed the adventure!";
            }
            else{
            timeUseText.text = GetFeedback(time)+ $" You completed the adventure in {time}!";
            }

        }

        //default selected button
        EventSystem.current.SetSelectedGameObject(keepPlayingButton.gameObject);

    }


    public void restartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void BackToLevelSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); 
    }

}

