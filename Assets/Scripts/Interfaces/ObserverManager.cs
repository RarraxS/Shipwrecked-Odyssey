using System.Collections.Generic;
using UnityEngine;

public class ObserverManager : MonoBehaviour
{
    #region Singleton Management

    private static ObserverManager instance;

    public static ObserverManager Instance
    { 
        get { return instance; } 
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(this);
    }

    #endregion

    private readonly List<IObserver> observers = new();

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObserver(string eventInfo)
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify(eventInfo);
        }
    }
}
