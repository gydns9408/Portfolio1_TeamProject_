using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class NewButtonSceneChange : MonoBehaviour
{
    Button newButton;
    Button LoadGame;

    // 게임 종료 처리용 버튼
    Button gameExitButton;
    Image gameExitPanel;
    Button exitButton;
    Button returnButton;

    // 메뉴 활성화 확인용 변수
    bool exitMenuClosed = true;

    public Action whatIsYOurNAme;

    private void Awake()
    {
        newButton = transform.GetChild(0).GetComponent<Button>();
        LoadGame = transform.GetChild(1).GetComponent<Button>();

        // 게임 종료 처리용
        gameExitButton = transform.GetChild(2).GetComponent<Button>();              // 스타트씬 오른쪽 위 버튼
        gameExitPanel = transform.GetChild(3).GetComponent<Image>();                // 패널
        exitButton = transform.GetChild(3).GetChild(1).GetComponent<Button>();      // 종료
        returnButton = transform.GetChild(3).GetChild(2).GetComponent<Button>();    // 돌아가기
    }

    private void Start()
    {
        gameExitPanel.gameObject.SetActive(false);  // 패널 꺼두기
        exitMenuClosed = true; // 패널 닫힌 상태
    }

    private void OnEnable()
    {
        newButton.onClick.AddListener(StartGameFunction);
        LoadGame.onClick.AddListener(LoadGameFunction);

        // 게임종료 처리용 버튼 기능
        gameExitButton.onClick.AddListener(OnExitPanel);
        exitButton.onClick.AddListener(GameExit);
        returnButton.onClick.AddListener(ReturnGame);
    }


    private void LoadGameFunction()
    {

        //로드씬 델리게이트(세이브 값 가져오기)
        DataController.Instance.LoadGameData();
        SceneManager.LoadScene(2);
    }

    void StartGameFunction()
    {
        DataController.Instance.DeleteSaveFile();
        whatIsYOurNAme?.Invoke();
    }

    // 게임 종료 버튼(오른쪽 위) 게임 종료 패널 활성화/비활성화
    private void OnExitPanel()
    {
        if(exitMenuClosed == true)  // 패널 닫혀있으면
        {
            gameExitPanel.gameObject?.SetActive(true);  // 활성화
            exitMenuClosed = !exitMenuClosed;   // 활성화 표시
        }
        else
        {
            // 패널 열려있으면
            gameExitPanel.gameObject?.SetActive(false); // 비활성화
            exitMenuClosed = !exitMenuClosed;   // 비활성화 표시
        }
    }

    // 게임 종료
    private void GameExit()
    {
        Application.Quit();     // 게임 종료
        Debug.Log("게임 종료");
    }

    // 게임으로 돌아가기
    private void ReturnGame()
    {
        // 패널 비활성화
        gameExitPanel.gameObject.SetActive(false);
        exitMenuClosed = true;
    }
}
