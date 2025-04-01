using UnityEngine;

public class TimeSheet : MonoBehaviour
{
    public byte days,
        hours, 
        minutes;

    public enum Season 
    { 
        Spring, 
        Summer, 
        Autum, 
        Winter 
    }

    public Season season;

    public byte minutesInAnHour, 
        hoursInADay,
        daysInAMonth,
        wakingUpTime, 
        sleepingTime, 
        minutesToAdd,
        timeBetweenUpdates;
}
