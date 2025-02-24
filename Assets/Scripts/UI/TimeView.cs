using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeView : MonoBehaviour
{
    [SerializeField] private ScripteableEventDoubleParameterInt timeHasChangedEvent;

    void Start()
    {
        timeHasChangedEvent.UnityAction += UpdateTime;
    }

    void Update()
    {
        
    }

    private void UpdateTime(int _hours, int _minutes)
    {
        Debug.Log("Hola");
    }
}
