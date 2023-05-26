using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayInfo : MonoBehaviour
{
    TextMeshProUGUI recordText;
    TextMeshProUGUI recordText2;
    TextMeshProUGUI recordText3;

    SaveBoardUI pauseMenu;

    private void Awake()
    {
        pauseMenu = FindObjectOfType<SaveBoardUI>();
        // 컴포넌트 찾기
        Transform child = transform.GetChild(0);
        recordText = child.GetComponent<TextMeshProUGUI>();
        Transform child1 = transform.GetChild(1);
        recordText2 = child1.GetComponent<TextMeshProUGUI>();
        Transform child3 = transform.GetChild(2);
        recordText3 = child3.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        pauseMenu.updateData += SetData;
    }

    public void SetData()
    {
        recordText.text = DataController.Instance.gameData.playerName;
        recordText2.text = DataController.Instance.gameData.currentDay.ToString();
        recordText3.text = DataController.Instance.gameData.currentTime.ToString();
    }
}
