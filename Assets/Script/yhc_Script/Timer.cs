using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;
using static PlayerBase;

public class Timer : MonoBehaviour
{
    SaveBoardUI pauseMenu;
    TextMeshProUGUI timerText;
    Sunshine sunshine;
    public int timeSpeed = 1;

    public int day;
    public int hour;

    private void Awake()
    { 
        timerText = GetComponent<TextMeshProUGUI>();
        sunshine = FindObjectOfType<Sunshine>();
        pauseMenu = FindObjectOfType<SaveBoardUI>();
    }

    private void Start()
    {
        sunshine.HourChange += OnHourChange;
        pauseMenu.updateData += setData;
        if (DataController.Instance.WasSaved == false)
        {
            PreInitialize();
        }
        else
        {
            Initialize();
        }
        timerText.text = $"Day : {day} \nHour : {hour}"; // 시계 초기값 1일차 6시로 설정
    }
    private void PreInitialize()
    {
        day = 1;
        hour = 6;
    }

    private void Initialize()
    {
        day = DataController.Instance.gameData.currentDay;
        hour = DataController.Instance.gameData.currentTime;
    }


    private void OnHourChange()
    {
        hour = hour + 1;
        if (hour > 23)
        {
            day++;
            hour = 0;
        }
        timerText.text = $"Day : {day} \nHour : {hour}";
    }
    private void setData()
    {
        DataController.Instance.gameData.currentDay = day;
        DataController.Instance.gameData.currentTime = hour;
    }
}