using UnityEngine;

public class TimeSheet : MonoBehaviour
{
    public int days,
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

    public int minutesInAnHour, 
        hoursInADay,
        daysInAMonth,
        wakingUpTime, 
        sleepingTime, 
        minutesToAdd,
        timeBetweenUpdates;
}
