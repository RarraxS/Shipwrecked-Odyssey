using System;
using UnityEngine;
using static TimeSheet;

public class TimeController : MonoBehaviour
{
    [SerializeField] private TimeSheet timeSheet;

    [SerializeField] private ScriptableEventSingleParameterBool isSleepingTimeEvent;
    [SerializeField] private ScriptableEventSingleParameterByte dayHasChangedEvent;
    [SerializeField] private ScriptableEventDoubleParameterByte timeHasChangedEvent;

    private int seasonsEnumLenght;
    private float timeContainer;

    private void OnEnable()
    {
        CallHasTimeChangedEvent();
        CallHasDayChangedEvent();
    }

    private void Start()
    {
        timeContainer = timeSheet.timeBetweenUpdates;

        seasonsEnumLenght = Enum.GetValues(typeof(TimeSheet.Season)).Length;

        isSleepingTimeEvent.UnityAction += CheckSeason;
        isSleepingTimeEvent.UnityAction += SetNewDay;
    }

    private void Update()
    {
        UpdateTimeTimer();
    }

    private void UpdateTimeTimer()
    {
        timeContainer -= Time.deltaTime;

        CheckTimePassed();
    }

    private void CheckTimePassed() 
    {
        if (IsTimeContainerFinished())
        {
            UpdateMinutes();

            timeContainer = timeSheet.timeBetweenUpdates;

            CallHasTimeChangedEvent();
        }
    }

    private void UpdateMinutes()
    {
        timeSheet.minutes += timeSheet.minutesToAdd;

        if (IsHourOver())
        {
            timeSheet.minutes -= timeSheet.minutesInAnHour;

            UpdateHours();
        }
    }

    private void UpdateHours()
    {
        timeSheet.hours += 1;

        if (IsDayFinished())
        {
            timeSheet.hours -= timeSheet.hoursInADay;

            UpdateDays();
        }

        SleepingTime();
    }

    private void UpdateDays()
    {
        timeSheet.days += 1;

        CallHasDayChangedEvent();

        if (IsMonthOver())
            timeSheet.days -= timeSheet.daysInAMonth;
    }

    private void SleepingTime()
    {
        if (IsSleepingTime())
            CallIsSleepingTimeEvent();
    }

    private void SetNewDay(bool _param)
    {
        timeSheet.hours = timeSheet.wakingUpTime;
        timeSheet.minutes = 0;
    }

    private void CheckSeason(bool _param)
    {
        if (IsMonthOver())
            UpdateSeason();
    }

    private void UpdateSeason()
    {
        timeSheet.season++;

        timeSheet.season = (Season)((int)timeSheet.season % seasonsEnumLenght);
    }

    private bool IsTimeContainerFinished()
    {
        return timeContainer <= 0;
    }

    private bool IsHourOver()
    {
        return timeSheet.minutes >= timeSheet.minutesInAnHour;
    }

    private bool IsDayFinished()
    {
        return timeSheet.hours >= timeSheet.hoursInADay;
    }

    private bool IsMonthOver() 
    {
        return timeSheet.days > timeSheet.daysInAMonth;
    }

    private bool IsSleepingTime()
    {
        return timeSheet.hours == timeSheet.sleepingTime;
    }

    private void CallIsSleepingTimeEvent()
    {
        isSleepingTimeEvent?.Invoke(true);
    }

    private void CallHasTimeChangedEvent()
    {
        timeHasChangedEvent?.Invoke(timeSheet.hours, timeSheet.minutes);
    }

    private void CallHasDayChangedEvent()
    {
        dayHasChangedEvent?.Invoke(timeSheet.days);
    }
}