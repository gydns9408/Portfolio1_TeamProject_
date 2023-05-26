using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    Button resume;
    Button save;
    Button main;
    Button option;
    Button menuClosedButton;    // 퍼즈 메뉴 닫기 버튼 추가
    OptionMenu optionMenu;
    PlayerUI playerUI;


    public Action onSave;


    private void Awake()
    {
        /*resume = GameObject.Find("Resume").GetComponent<Button>();
        save = GameObject.Find("Save").GetComponent<Button>();
        main = GameObject.Find("Main").GetComponent<Button>();
        option = GameObject.Find("Option").GetComponent<Button>();
        optionMenu = GameObject.Find("OptionMenu").GetComponent<OptionMenu>();*/
        resume = gameObject.transform.GetChild(1).GetComponent<Button>();
        save = gameObject.transform.GetChild(3).GetComponent<Button>();
        main = gameObject.transform.GetChild(4).GetComponent<Button>();
        option = gameObject.transform.GetChild(2).GetComponent<Button>();
        optionMenu = gameObject.transform.GetChild(5).GetComponent<OptionMenu>();
        menuClosedButton = gameObject.transform.GetChild(6).GetComponent<Button>(); // 퍼즈 메뉴 닫기버튼
        playerUI = FindObjectOfType<PlayerUI>();
    }

    private void Start()
    {
        /*resume.onClick.AddListener(ResumeGame);
        save.onClick.AddListener(CallSaveMenu);
        main.onClick.AddListener(CallMainMenu);
        option.onClick.AddListener(CallOptionMenu);
        menuClosedButton.onClick.AddListener(ClosePauseMenu);   // 퍼즈 메뉴 닫기버튼*/
        gameObject.SetActive(false);
        optionMenu.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        resume.onClick.AddListener(ResumeGame);
        main.onClick.AddListener(CallMainMenu);
        option.onClick.AddListener(CallOptionMenu);
        menuClosedButton.onClick.AddListener(ClosePauseMenu);   // 퍼즈 메뉴 닫기버튼
        save.onClick.AddListener(CallSaveMenu);
    }


    private void ResumeGame()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void CallSaveMenu()
    {
        onSave?.Invoke();
    }

    private void CallMainMenu()
    {
        Time.timeScale = 1.0f;      // 일시정지 해제
        SceneManager.LoadScene(0);  // 0번씬으로
    }

    private void CallOptionMenu()
    {
        optionMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// 퍼즈 메뉴에 닫기 버튼 추가한 것
    /// </summary>
    private void ClosePauseMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        playerUI.menuClosed = !playerUI.menuClosed;
    }
}
