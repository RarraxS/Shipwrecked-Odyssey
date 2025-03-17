using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Scriptable Event Double Parameter Int", menuName = "Scriptable Event/Double Parameter/Int")]
public class ScriptableEventDoubleParameterInt : ScriptableObject
{
    public UnityAction<int, int> UnityAction;

    public void Invoke(int _value1, int _value2)
    {
        this.UnityAction?.Invoke(_value1, _value2);
    }
}