using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimerSubject : MonoBehaviour
{
    private List<TimerObserver> _observers = new List<TimerObserver>();
    

    public void AddObserver(TimerObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(TimerObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyTime(float time)
    {
        foreach (TimerObserver observer in _observers)
        {
            observer.UpdateTimeObserver(time);
        }
    }

    public void NotifyIsRunning(bool isRunning){
        foreach (TimerObserver observer in _observers)
        {
            observer.UpdateTimeRunning(isRunning);
        }
    }

    public void NotifyTimesUp(bool timesUp){
        foreach (TimerObserver observer in _observers)
        {
            observer.TimesUP(timesUp);
        }
    }
}
