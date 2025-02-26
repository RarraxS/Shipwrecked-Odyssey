using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeView : MonoBehaviour
{
    [SerializeField] private ScripteableEventDoubleParameterInt timeHasChangedEvent;

    [SerializeField] private TMP_Text timeText; 

    void Start()
    {
        timeHasChangedEvent.UnityAction += UpdateTime;
    }

    void Update()
    {
        
    }

    private void UpdateTime(int _hours, int _minutes)
    {
        Debug.Log("Hola Carolo, perraco; Shei, si lees esto rata :P");

        timeText.text = _hours.ToString() + ":" + _minutes.ToString("00");
    }
}
