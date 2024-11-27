using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialPopUp : MonoBehaviour
{

    public GameObject closeButton;
    public float enableDelay = 2f;

    private void Start()
    {
        if (closeButton != null)
        {
            StartCoroutine(SelectButtonAfterDelay());
        }
    }

    IEnumerator SelectButtonAfterDelay()
    {
        Debug.Log("Kill me");
        yield return new WaitForSecondsRealtime(enableDelay);

        Debug.Log("Updating Close Button");
        // Set the button as the current selected object in the EventSystem
        closeButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
    }

    public void closePopUp()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
