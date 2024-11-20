using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    // Buttons for the pause menu
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button closeControlsButton;
    [SerializeField] private GameObject mainPauseButtons;
    [SerializeField] private GameObject controlPage;
    [SerializeField] private EventSystem eventSystem;

    // List to store all menu buttons
    private List<Button> menuButtons;
    private int selectedButtonIndex = 0; // Track currently selected button

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Pause.Enable();
        playerInputActions.Player.Pause.performed += OnPause;
    }

    void Start()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);

        // Initialize the list of buttons in the order you want them to be navigated
        menuButtons = new List<Button> { resumeButton, restartButton, mainMenuButton };

    }

    void OnEnable()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Player.Pause.Enable();
        }
    }


    void OnPause(InputAction.CallbackContext context)
    {
        if (this.gameObject != null)
        {
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            Debug.Log("Unpaused");
        }
        else
        {
            Time.timeScale = 0;
            Debug.Log("Paused");
        }
    }

    public void restartGame()
    {
        playerInputActions.Player.Pause.Disable();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void backToMainMenu()
    {
        playerInputActions.Player.Pause.Disable();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void resumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void openControls()
    {
        mainPauseButtons.SetActive(false);
        controlPage.SetActive(true);
        EventSystem.current.SetSelectedGameObject(closeControlsButton.gameObject);

    }

    public void closeControls()
    {
        mainPauseButtons.SetActive(true);
        controlPage.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton.gameObject);
    }
}
