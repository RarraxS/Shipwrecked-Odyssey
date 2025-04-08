using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Scriptable Event Double Parameter Byte", menuName = "Scriptable Event/Double Parameter/Byte")]
public class ScriptableEventDoubleParameterByte : ScriptableObject
{
    public UnityAction<byte, byte> UnityAction;

    public void Invoke(byte _value1, byte _value2)
    {
        this.UnityAction?.Invoke(_value1, _value2);
    }
}