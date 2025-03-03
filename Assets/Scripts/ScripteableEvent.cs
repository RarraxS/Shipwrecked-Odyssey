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

[CreateAssetMenu(fileName = "Scripteable Event Single Parameter Bool", menuName = "Scripteable Event/Single Parameter/Bool")]
public class ScripteableEventSingleParameterBool : ScripteableEventSingleParameter<bool> { }

[CreateAssetMenu(fileName = "Scripteable Event Single Parameter Int", menuName = "Scripteable Event/Single Parameter/Int")]
public class ScripteableEventSingleParameterInt : ScripteableEventSingleParameter<int> { }


[CreateAssetMenu(fileName = "Scripteable Event Double Parameter Int", menuName = "Scripteable Event/Double Parameter/Int")]
public class ScripteableEventDoubleParameterInt : ScripteableEventDoubleParameter<int> { }