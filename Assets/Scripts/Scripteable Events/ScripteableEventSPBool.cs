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