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

    // STRING interface --------------------------------------------------------------------

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

    // STRING and INT interface --------------------------------------------------------------

    private readonly List<IObserverNum> observersNum = new();

    public void AddObserverNum(IObserverNum observer)
    {
        observersNum.Add(observer);
    }

    public void RemoveObserverNum(IObserverNum observer)
    {
        observersNum.Remove(observer);
    }

    public void NotifyObserverNum(string eventInfo, int numInfo)
    {
        foreach (IObserverNum observer in observersNum)
        {
            observer.OnNotify(eventInfo, numInfo);
        }
    }
}
