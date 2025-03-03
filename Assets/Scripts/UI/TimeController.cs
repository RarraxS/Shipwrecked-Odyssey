using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static TimeSheet;

public class TimeController : MonoBehaviour
{
    [SerializeField] private TimeSheet timeSheet;

    [SerializeField] private ScripteableEventSingleParameterBool isSleepingTimeEvent;
    [SerializeField] private ScripteableEventSingleParameterInt dayHasChangedEvent;
    [SerializeField] private ScripteableEventDoubleParameterInt timeHasChangedEvent;

    private float timeContainer;

    private int seasonsEnumLenght;

    private void OnEnable()
    {
        CallHasTimeChangedEvent();
        CallHasDayChangedEvent();
    }

    private void Start()
    {
        timeContainer = timeSheet.timeBetweenUpdates;

        seasonsEnumLenght = Enum.GetValues(typeof(TimeSheet.Season)).Length;

        //_timeSheet.season = _timeSheet.season += 2;
        //Debug.Log(_timeSheet.season);
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

    private bool IsTimeContainerFinished()
    {
        return timeContainer <= 0;
    }

    private void UpdateMinutes()
    {
        timeSheet.minutes += timeSheet.minutesToAdd;

        if (timeSheet.minutes >= timeSheet.minutesInAnHour)
        {
            timeSheet.minutes -= timeSheet.minutesInAnHour;

            UpdateHours();
        }
    }

    private void UpdateHours()
    {
        timeSheet.hours += 1;

        if (timeSheet.hours >= timeSheet.hoursInADay)
        {
            timeSheet.hours -= timeSheet.hoursInADay;

            UpdateDays();
        }

        IsSleepingTime();
    }

    private void UpdateDays()
    {
        timeSheet.days += 1;

        CallHasDayChangedEvent();

        if (timeSheet.days > timeSheet.daysInAMonth)
            timeSheet.days -= timeSheet.daysInAMonth;
    }

    private void SetNewDay()
    {
        timeSheet.hours = timeSheet.wakingUpTime;
        timeSheet.minutes = 0;
    }

    private void IsSleepingTime()
    {
        if (timeSheet.hours == timeSheet.sleepingTime)
        {
            CallIsSleepingTimeEvent();
            CheckSeason();
            SetNewDay();
        }
    }

    private void CheckSeason()
    {
        if (timeSheet.days > timeSheet.daysInAMonth)
            UpdateSeason();
    }

    private void UpdateSeason()
    {
        timeSheet.season++;

        timeSheet.season = (Season)((int)timeSheet.season % seasonsEnumLenght);
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