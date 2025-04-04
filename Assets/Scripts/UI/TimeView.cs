using UnityEngine;
using TMPro;

public class TimeView : MonoBehaviour
{
    [SerializeField] private ScriptableEventDoubleParameterInt timeHasChangedEvent;
    [SerializeField] private ScriptableEventSingleParameterInt dayHasChangedEvent;

    [SerializeField] private TMP_Text timeText, daysText;

    [SerializeField] private string prefixDaysText;

    private void Awake()
    {
        timeHasChangedEvent.UnityAction += UpdateTime;
        dayHasChangedEvent.UnityAction += UpdateDay;
    }

    private void UpdateTime(int _hours, int _minutes)
    {
        //Debug.Log("Hola Carolo, perraco; Shei, si lees esto rata :P");

        timeText.text = _hours.ToString() + ":" + _minutes.ToString("00");
    }

    private void UpdateDay(int _day)
    {
        //Debug.Log("Hola Carolo, perraco; Shei, si lees esto rata :P");

        daysText.text = prefixDaysText + _day.ToString();
    }
}
