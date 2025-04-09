using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Scriptable Event Single Parameter Float", menuName = "Scriptable Event/Single Parameter/float")]
public class ScriptableEventSingleParameterFloat : ScriptableObject
{
    public UnityAction<float> UnityAction;

    public void Invoke(float _value)
    {
        this.UnityAction?.Invoke(_value);
    }
}