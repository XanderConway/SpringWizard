using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private PlayerInputActions playerInputActions;
    private List<Button> menuButtons;
    private int selectedButtonIndex = 0; // Track the currently selected button

    private float buttonPressCooldown = 0.5f; // Cooldown time in seconds
    private float lastButtonPressTime = 0f;   // Time the last button was pressed

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        StartCoroutine(EnableInputsAfterDelay(0.1f)); 

    }
    IEnumerator EnableInputsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Inputs enabled after delay");
        // Re-enable input after delay

        playerInputActions.Player.Navigate.Enable();
        playerInputActions.Player.Navigate.performed += OnNavigate;
        playerInputActions.Player.Select.Enable();
        playerInputActions.Player.Select.performed += OnSelect;
    }

    void Start()
    {
        // Initialize the list of buttons
        menuButtons = new List<Button> { playButton, settingsButton, quitButton };

        // Highlight the first button by default
        HighlightButton(selectedButtonIndex);


    }

    void OnDisable()
    {
        if (playerInputActions != null)
        {
            playerInputActions.Player.Navigate.Disable();
            playerInputActions.Player.Select.Disable();
        }
    }

    // Handle navigation (up/down arrow keys or joystick)
    void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();

        if (inputDirection.y > 0) // Up arrow or joystick up
        {
            MoveSelection(-1); // Move up in the menu
        }
        else if (inputDirection.y < 0) // Down arrow or joystick down
        {
            MoveSelection(1); // Move down in the menu
        }
    }

    // Move the selection up or down in the menu
    private void MoveSelection(int direction)
    {
        // Update the selected button index, cycling through the list
        selectedButtonIndex = (selectedButtonIndex + direction + menuButtons.Count) % menuButtons.Count;

        // Highlight the new selected button
        HighlightButton(selectedButtonIndex);
    }

    // Highlight the selected button
    private void HighlightButton(int index)
    {
        if (menuButtons != null && menuButtons.Count > 0 && menuButtons[index] != null)
        {
            Button button = menuButtons[index];
            button.Select(); // Highlight the button visually
        }
        else
        {
            Debug.LogError("Button at index " + index + " is null or not assigned.");
        }
    }


    // Handle the selection (e.g., pressing Enter or action button) with a debounce mechanism
    void OnSelect(InputAction.CallbackContext context)
    {
        // Check if enough time has passed since the last button press
        if (Time.time - lastButtonPressTime >= buttonPressCooldown)
        {
            // Update the last button press time
            lastButtonPressTime = Time.time;

            // Simulate clicking the currently selected button
            menuButtons[selectedButtonIndex].onClick.Invoke();
        }
    }

    // Button actions
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
