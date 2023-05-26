using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    int day;
    public int Day 
    {
        get => day; 
        set => day = value;
    }
    int hour;
    public int Hour 
    {
        get => hour;
        set => hour = value;
    }
    float minute;
    public float Minute 
    {
        get => minute;
        set => minute = value;
    }
    // Start is called before the first frame updat

    // Update is called once per frame
    void Update()
    {
        minute += Time.deltaTime * 2;
        if (minute >= 60) 
        {
            minute -= 60;
            hour += 1;
            if (hour >= 24) 
            { 
                hour -= 24;
                day++;
            }
        }
        //Debug.Log($"{Day}¿œ {Hour}Ω√ {Minute}∫–");
    }
}
