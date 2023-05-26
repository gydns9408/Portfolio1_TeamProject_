using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    Slider HPUI;
    PlayerBase player;
    Button pauseButton;
    PauseMenu pauseMenu;
    UIAct uikey;
    OptionMenu optionMenu;
    Transform diePanel;
    Button returnMainButton;
    Transform keySet;
    Transform help;

    public bool menuClosed;
    bool keySetOn = false;

    private void Awake()
    {
        player = FindObjectOfType<PlayerBase>();
        HPUI = GetComponentInChildren<Slider>();
        pauseButton = GameObject.Find("PauseButton").GetComponent<Button>();
        pauseMenu = GameObject.Find("PauseMenu").GetComponent<PauseMenu>();
        optionMenu = FindObjectOfType<OptionMenu>();
        keySet = transform.GetChild(4);
        diePanel = transform.GetChild(5);
        help = transform.GetChild(6);
        returnMainButton = diePanel.GetComponentInChildren<Button>();

        uikey = new UIAct();
    }


    private void Start()
    {
        player = FindObjectOfType<PlayerBase>();
        menuClosed = true;
        
        diePanel.gameObject.SetActive(false);
        keySet.gameObject.SetActive(false);

        player.onUpgradeHp += RefreshHPSlider;
        player.onDie += OnDiePanel;
        pauseButton.onClick.AddListener(CallPauseMenu);
        returnMainButton.onClick.AddListener(CallMainMenu);

        StartCoroutine(OnHelpPanel());
    }

    private void OnEnable()
    {
        uikey.UI.Enable();
        uikey.UI.Esc.performed += ESC;
        uikey.UI.KeySet.performed += OnKeySet;
    }


    private void OnDisable()
    {
        uikey.UI.KeySet.performed -= OnKeySet;
        uikey.UI.Esc.performed -= ESC;
        uikey.UI.Disable();
    }

    private void ESC(InputAction.CallbackContext _)
    {
        if(menuClosed)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0.0f;      // 게임 일시 정지
            menuClosed = !menuClosed;
        }
        else if(!menuClosed)
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1.0f;      // 일시 정지 해제
            menuClosed = !menuClosed;
        }
    }

    private void CallPauseMenu()
    {
        if (pauseMenu != null && menuClosed)
        {
            pauseMenu.gameObject.SetActive(true);
            Time.timeScale = 0.0f;      // 게임 일시 정지
            menuClosed = !menuClosed;
        }
        else if( pauseMenu != null && !menuClosed ) 
        {
            pauseMenu.gameObject.SetActive(false);
            Time.timeScale = 1.0f;      // 일시 정지 해제
            menuClosed = !menuClosed;
        }
        else
        {
            Debug.LogWarning("퍼즈 메뉴 불러오기 실패");
        }
        Debug.Log("퍼즈");
    }

    // 사망패널 기능 코루틴으로 변경
    private void OnDiePanel()
    {
        StartCoroutine(OnDiePanelCor());
    }

    private void OnKeySet(InputAction.CallbackContext _)
    {
        if (keySetOn == false)
        {
            keySet.gameObject.SetActive(true);
            keySetOn = !keySetOn;
        }
        else
        {
            keySet.gameObject.SetActive(false);
            keySetOn = !keySetOn;
        }
    }

    /// <summary>
    /// 플레이어 사망시 메인 버튼의 기능
    /// </summary>
    private void CallMainMenu()
    {
        Time.timeScale = 1.0f;      // 메인버튼 누르면 일시정지 해제
        SceneManager.LoadScene(0);  // 죽고 난 후 main버튼 클릭시 0번씬으로 이동하도록 변경
    }


    IEnumerator OnHelpPanel()
    {
        help.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        help.gameObject.SetActive(false);
        StopCoroutine(OnHelpPanel());
    }
   
    /// <summary>
    /// 사망 패널 활성화용 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator OnDiePanelCor()
    {
        Debug.Log("코루틴 시작");
        yield return new WaitForSeconds(3.0f);
        diePanel.gameObject.SetActive(true);
        StopCoroutine(OnDiePanelCor());
    }

    private void RefreshHPSlider(float ratio)
    {
        HPUI.value = player.HP;
    }
}
