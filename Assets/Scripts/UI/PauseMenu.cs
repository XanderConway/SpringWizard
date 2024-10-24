using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    // Buttons for the pause menu
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;

    // List to store all menu buttons
    private List<Button> menuButtons;
    private int selectedButtonIndex = 0; // Track currently selected button

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Pause.Enable();
        playerInputActions.Player.Pause.performed += OnPause;

        // Enable button navigation
        playerInputActions.Player.Navigate.Enable();
        playerInputActions.Player.Navigate.performed += OnNavigate;

        playerInputActions.Player.Select.Enable();
        playerInputActions.Player.Select.performed += OnSelect;
    }

    void Start()
    {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);

        // Initialize the list of buttons in the order you want them to be navigated
        menuButtons = new List<Button> { resumeButton, restartButton, mainMenuButton };

        //playerInputActions = new PlayerInputActions();
        //playerInputActions.Player.Pause.Enable();
        //playerInputActions.Player.Pause.performed += OnPause;

        //// Enable button navigation
        //playerInputActions.Player.Navigate.Enable();
        //playerInputActions.Player.Navigate.performed += OnNavigate;

        //playerInputActions.Player.Select.Enable();
        //playerInputActions.Player.Select.performed += OnSelect;

    }

    void OnEnable()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Player.Pause.Enable();
            playerInputActions.Player.Navigate.Enable();
            playerInputActions.Player.Select.Enable();
        }
    }

    void OnDisable()
    {
        playerInputActions.Player.Navigate.Disable();
        playerInputActions.Player.Select.Disable();
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

    // Handle navigation (up/down) using arrow keys or joystick
    void OnNavigate(InputAction.CallbackContext context)
    {
        Debug.Log("navigating");
        Vector2 inputDirection = context.ReadValue<Vector2>();

        if (inputDirection.y > 0) // Up arrow or up on joystick
        {
            MoveSelection(-1); // Move up in the menu
        }
        else if (inputDirection.y < 0) // Down arrow or down on joystick
        {
            MoveSelection(1); // Move down in the menu
        }
    }

    // Move the selection up or down
    private void MoveSelection(int direction)
    {
        // Calculate new selected button index
        selectedButtonIndex = (selectedButtonIndex + direction + menuButtons.Count) % menuButtons.Count;

        Debug.Log("selectedIndex " + selectedButtonIndex);

        // Highlight the new selected button
        HighlightButton(selectedButtonIndex);
    }

    // Highlight the selected button
    private void HighlightButton(int index)
    {
        if (menuButtons != null && menuButtons.Count > 0 && menuButtons[index] != null)
        {
            Debug.Log("Highlighting " + index);
            Button button = menuButtons[index];
            button.Select(); // Highlight the button visually
        }
        else
        {
            Debug.LogError("Button at index " + index + " is null or not assigned.");
        }
    }


    // Handle button selection (e.g., pressing Enter or the action button)
    void OnSelect(InputAction.CallbackContext context)
    {
        Debug.Log("Selecting");
        // Simulate clicking the currently selected button
        menuButtons[selectedButtonIndex].onClick.Invoke();


    }

    public void restartGame()
    {
        playerInputActions.Player.Pause.Disable();

        playerInputActions.Player.Navigate.Disable();

        playerInputActions.Player.Select.Disable();


        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void backToMainMenu()
    {
        playerInputActions.Player.Pause.Disable();

        playerInputActions.Player.Navigate.Disable();

        playerInputActions.Player.Select.Disable();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void resumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
