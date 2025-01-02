using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayCicle : MonoBehaviour, IObserver
{
    [SerializeField] private Light2D sunlight;
    [SerializeField] private List<DayStates> sunnyDayStates;

    [SerializeField] private float rainingProbability;
    [SerializeField] private List<DayStates> rainyDayStates;
    [SerializeField] private GameObject rain;
    private bool raining = false;

    private float timeTimer;

    private int indexDayState = 0;

    void Start()
    {
        CheckRain();

        ObserverManager.Instance.AddObserver(this);

        sunlight = GetComponent<Light2D>();

        timeTimer = HudHora.Instance.timeTimer;
    }


    void Update()
    {
        if (raining == false)
            CheckDayState(sunnyDayStates);

        else
            CheckDayState(rainyDayStates);
    }

    private void CheckDayState(List<DayStates> dayStates)
    {
        if (indexDayState == dayStates.Count - 1)
            return;

        else if (IsHourOnCurrentDayStateIndex(dayStates))
        {
            float totalMinutes = CalculateTotalMinutes(dayStates);

            float minutesRemaining = CalculateMinutesRemaining(totalMinutes, dayStates);

            ApplyIlumination(minutesRemaining, totalMinutes, dayStates);
        }

        else
            indexDayState++;
    }

    private bool IsHourOnCurrentDayStateIndex(List<DayStates> dayStates)
    {
        return dayStates[indexDayState].hour <= HudHora.Instance.hour && 
               dayStates[indexDayState + 1].hour > HudHora.Instance.hour;
    }

    private float CalculateTotalMinutes(List<DayStates> dayStates)
    {
        return ((dayStates[indexDayState + 1].hour - dayStates[indexDayState].hour) * 6) * timeTimer;
    }

    private float CalculateMinutesRemaining(float totalMinutes, List<DayStates> dayStates)
    {
        float hoursRemaining = dayStates[indexDayState + 1].hour - HudHora.Instance.hour;

        float minutesRemaining = hoursRemaining * 6;

        if (HudHora.Instance.minutos != 0)
        {
            minutesRemaining -= HudHora.Instance.minutos / 10;
        }

        return (totalMinutes - (minutesRemaining * timeTimer)) - HudHora.Instance.timeTimer;
    }

    private void ApplyIlumination(float minutesRemaining, float totalMinutes, List<DayStates> dayStates)
    {
        float time = minutesRemaining / totalMinutes;

        sunlight.color = Color.Lerp(dayStates[indexDayState].ilumination, dayStates[indexDayState + 1].ilumination, time);
    }

    private void CheckRain()
    {
        rain.SetActive(raining);
    }

    public void OnNotify(string eventInfo)
    {
        if (eventInfo == "Day completed")
        {
            indexDayState = 0;

            float randomNumber = UnityEngine.Random.Range(0f, 100f);

            if (randomNumber <= rainingProbability)
            {
                raining = true;
                ObserverManager.Instance.NotifyObserver("Rainy day");
            }

            else
                raining = false;

            CheckRain();
        }
    }
}


[Serializable]
public class DayStates
{
    public int hour;

    public Color ilumination;
}