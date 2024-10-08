using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<TrickObserver> _observers = new List<TrickObserver>();

    public void AddObserver(TrickObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(TrickObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(PlayerTricks playerTricks)
    {
        foreach (TrickObserver observer in _observers)
        {
            observer.UpdateObserver(playerTricks);
        }
    }
}
