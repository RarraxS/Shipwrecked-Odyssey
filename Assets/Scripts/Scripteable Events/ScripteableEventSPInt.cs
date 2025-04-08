using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Scriptable Event Single Parameter Int", menuName = "Scriptable Event/Single Parameter/Int")]
public class ScriptableEventSingleParameterInt : ScriptableObject
{
    public UnityAction<int> UnityAction;

    public void Invoke(int _value)
    {
        this.UnityAction?.Invoke(_value);
    }
}