using UnityEngine;
using UnityEngine.Events;

public class ScripteableEventSingleParameter<T> : ScriptableObject
{
    public UnityAction<T> UnityAction;

    public void Invoke(T _value)
    {
        this.UnityAction?.Invoke(_value);
    }
}

public class ScripteableEventDoubleParameter<T> : ScriptableObject
{
    public UnityAction<T, T> UnityAction;

    public void Invoke(T _value1, T _value2)
    {
        this.UnityAction?.Invoke(_value1, _value2);
    }
}

[CreateAssetMenu(fileName = "Scripteable Event Double Parameter Int", menuName = "Events/Scriptable")]
public class ScripteableEventDoubleParameterInt : ScripteableEventDoubleParameter<int> { }
