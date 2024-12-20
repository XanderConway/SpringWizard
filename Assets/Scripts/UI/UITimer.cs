using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UITimer : TimerSubject
{
    public float countdownTime = 60f; //Default
    public AudioSource audioSource;
    public AudioClip lowTimeClip;
    public float musicChangeThreshold = 20f;

    private bool clipPlayed = false;

    public float currentTime;
    private bool isTimerRunning = false;


    //UI elements
    public TextMeshProUGUI timerText;

    private bool stopTimer;


    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;

            NotifyTime(currentTime);  // Notify observers of remaining time
            UpdateTimerUI(currentTime);


            if(currentTime < musicChangeThreshold && audioSource != null)
            {
                audioSource.pitch = 1.2f;

                if(!clipPlayed)
                {
                    if(lowTimeClip != null)
                    {
                        audioSource.PlayOneShot(lowTimeClip);
                    }
                    clipPlayed = true;
                }
            }

            //if (currentTime <= 0)
            //{
            //    currentTime = 0;
            //    isTimerRunning = false;
            //    TimerEnded();
                
            //}

            NotifyIsRunning(isTimerRunning);
        }

    }


    //TODO start time when the ui is enable for now 
    void OnEnable()
    {
        StartTimer();
    }



    public void StartTimer(){

        isTimerRunning = true;

        if (LevelManager.Instance != null && LevelManager.Instance.currentLevel != null)
        {
            countdownTime = LevelManager.Instance.currentLevel.timeLimit;
            
            if(LevelManager.Instance.currentIsPractice)
            {
                isTimerRunning = false;
                timerText.text = "Practice";

            }
        }
        currentTime = 0;


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

        SceneManager.LoadScene("EndMenu");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    //getter for time passed
    public float GetTimePassed()
    {
        return currentTime;
    }
    


    //UI functions

    private void UpdateTimerUI(float remainingTime)
    {
        // Update the UI with the remaining time
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTime);
        string formatTimer = timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00");
        timerText.text = formatTimer;
    }
}


