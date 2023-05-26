using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[Serializable]
public class GameData
{

    public string playerName;         //이름
    public int playerHp;
    public Vector3 playerPosition;      //위치

    public int[] itemCount;             // 갯수
    public ItemType[] itemTypes;          // 종류
    public int[] toolType;

    public Vector3 workbenchPosition;   //제작대 위치

    public int currentToolItem;
    public int toolLevel;         // 장착한 장비아이템 레벨

    public int currentDay;         // 날짜
    public int currentTime;

    public float currentSunRotate;  // Sunshine의 회전값
    public bool currentIsNight;     // 낮, 밤 구분
    public float cubemap;           // 하늘
    public float fogDens;           // 밤의 어두운 정도

    public bool flowerIsFlower;     // 꽃의 생성
    public bool treeIsTree;         // 나무의 생성
}
