using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Scriptable Event Single Parameter Bool", menuName = "Scriptable Event/Single Parameter/Bool")]
public class ScriptableEventSingleParameterBool : ScriptableObject
{
    public UnityAction<bool> UnityAction;

    public void Invoke(bool _value)
    {
        this.UnityAction?.Invoke(_value);
    }
}

[CreateAssetMenu(fileName = "Scriptable Event Single Parameter Int", menuName = "Scriptable Event/Single Parameter/Int")]
public class ScriptableEventSingleParameterInt : ScriptableObject
{
    public UnityAction<int> UnityAction;

    public void Invoke(int _value)
    {
        this.UnityAction?.Invoke(_value);
    }
}


[CreateAssetMenu(fileName = "Scriptable Event Double Parameter Int", menuName = "Scriptable Event/Double Parameter/Int")]
public class ScriptableEventDoubleParameterInt : ScriptableObject
{
    public UnityAction<int, int> UnityAction;

    public void Invoke(int _value1, int _value2)
    {
        this.UnityAction?.Invoke(_value1, _value2);
    }
}