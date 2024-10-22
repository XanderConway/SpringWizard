using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSubject : MonoBehaviour
{
    private List<TrickObserver> observers = new List<TrickObserver>();

    public void AddObserver(TrickObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(TrickObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyTrickObservers(PlayerTricks playerTricks)
    {
        foreach (TrickObserver observer in observers)
        {
            observer.UpdateTrickObserver(playerTricks);
        }
    }
}
