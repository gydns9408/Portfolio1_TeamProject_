using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveBoardUI : MonoBehaviour
{
    // 세이브보드 버튼 관리용

    PauseMenu pauseMenu;

    Button saveCloseButton;

    public Action updateData;

    private void Awake()
    {
        saveCloseButton = transform.GetChild(2).GetComponent<Button>();
        pauseMenu =FindObjectOfType<PauseMenu>();
    }

    private void Start()
    {
        saveCloseButton.onClick.AddListener(CloseSaveBoard);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        pauseMenu.onSave += OpenSaveBoard;
    }

    private void OpenSaveBoard()
    {
        gameObject.SetActive(true);
        updateData?.Invoke();
    }

    // 버튼 기능 추가
    private void CloseSaveBoard()
    {
        gameObject.SetActive(false);
    }
}
