using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Scriptable Event Single Parameter Byte", menuName = "Scriptable Event/Single Parameter/Byte")]
public class ScriptableEventSingleParameterByte : ScriptableObject
{
    public UnityAction<byte> UnityAction;

    public void Invoke(byte _value1)
    {
        this.UnityAction?.Invoke(_value1);
    }
}