using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    [SerializeField] private TutorialPopUp tutorial;
    private bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!activated)
        {
            tutorial.gameObject.SetActive(true);
            Time.timeScale = 0;
            activated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
