using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITimer : TimerSubject
{
    public float countdownTime = 60f; //Default
    private float currentTime;
    private bool isTimerRunning = false;


    //UI elements
    public TextMeshProUGUI timerText;


    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning && currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            NotifyTime(currentTime);  // Notify observers of remaining time
            UpdateTimerUI(currentTime);

            if (currentTime <= 0)
            {
                currentTime = 0;
                isTimerRunning = false;
                TimerEnded();
                
            }

            NotifyIsRunning(isTimerRunning);
        }

    }


    //TODO start time when the ui is enable for now 
    void OnEnable()
    {
        StartTimer();
    }



    public void StartTimer(){
        currentTime = countdownTime;
        isTimerRunning = true;
        NotifyIsRunning(isTimerRunning);
    }

    public void StartTimer(float time)
    {
        currentTime = time;
        isTimerRunning = true;
        NotifyIsRunning(isTimerRunning);
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        NotifyIsRunning(isTimerRunning);
    }

    public float GetRemainingTime()
    {
        return currentTime;
    }

    private void TimerEnded()
    {
        NotifyTimesUp(true);
        timerText.text = "Time's up!";

        SceneManager.LoadScene(2);
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
    }


    //UI functions

    private void UpdateTimerUI(float remainingTime)
    {
        // Update the UI with the remaining time
        timerText.text = "Time Left: " + Mathf.CeilToInt(remainingTime).ToString();
    }
}
