public interface IObserver
{
    void OnNotify(string eventInfo);
}

public interface IObserverNum
{
    void OnNotify(string eventInfo, int numInfo);
}