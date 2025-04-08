using UnityEngine;
using TMPro;

public class TimeView : MonoBehaviour
{
    [SerializeField] private ScriptableEventDoubleParameterByte timeHasChangedEvent;
    [SerializeField] private ScriptableEventSingleParameterByte dayHasChangedEvent;

    [SerializeField] private TMP_Text timeText, daysText;

    [SerializeField] private string prefixDaysText;

    private void Awake()
    {
        timeHasChangedEvent.UnityAction += UpdateTime;
        dayHasChangedEvent.UnityAction += UpdateDay;
    }

    private void UpdateTime(byte _hours, byte _minutes)
    {
        timeText.text = _hours.ToString() + ":" + _minutes.ToString("00");
    }

    private void UpdateDay(byte _day)
    {
        daysText.text = prefixDaysText + _day.ToString();
    }
}
