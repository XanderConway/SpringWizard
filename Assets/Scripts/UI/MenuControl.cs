using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuControl : MonoBehaviour
{
    
    [SerializeField] private GameObject pauseMenu;
    private PlayerInputActions playerInputActions;
    
    //dont destroy on load
    private void Awake()
    {
      

    }

    // Start is called before the first frame update
    void Start()
    {
        // deactivates the pause menu
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

  
}
