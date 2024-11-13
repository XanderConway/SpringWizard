using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trickScoreText;
    [SerializeField] private string tutorialText;
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
        trickScoreText.gameObject.SetActive(true);
        trickScoreText.text = tutorialText;
    }

    private void OnTriggerExit(Collider other)
    {
        trickScoreText.gameObject.SetActive(false);
    }
}
