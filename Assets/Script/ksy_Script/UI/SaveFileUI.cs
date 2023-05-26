/*public class SaveFileUI : MonoBehaviour
{
    Button save1;

    PauseMenu startSave;

    PlayInfo playInfo;
    SetUpItem setUpItem;
    Sunshine sunshine;
    Timer timer;

    PlayerBase player;
    string playerName;         //이름
    int playerHp;
    float playerPositionX;      //위치
    float playerPositionY;
    float playerPositionZ;

    int[] itemCount = null;             // 갯수
    int[] itemTypes = new int[ItemManager.Instance.itemInventory.ItemTypeArray.Length];        // 종류

    float workbenchPositionX;   //제작대 위치
    float workbenchPositionY;
    float workbenchPositionZ;

    ToolItemTag toolTag;        // 장착한 장비아이템 이름
    int toolName;
    int toolLevel;         // 장착한 장비아이템 레벨

    int currentDay;         // 날짜 저장 배열
    int currentTime;

    float currentTimeX;     // 로테이션 값으로 현재 시간 저장
    float currentTimeY;     // 로테이션 값으로 현재 시간 저장
    float currentTimeZ;     // 로테이션 값으로 현재 시간 저장

    public Action<int> LoadHp;
    public Action<ToolItemTag, int> onChangeTool;
    public Action<Quaternion> onLoardRotate;
    public Action<int> onLoardDay;
    public Action<int> onLoardTime;
    private void Start()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
        LoadRankingData();
    }

    private void Awake()
    {
        setUpItem = FindObjectOfType<SetUpItem>();
        timer = GetComponent<Timer>();
        save1 = transform.GetChild(0).GetComponent<Button>();

        startSave = FindObjectOfType<PauseMenu>();

        player = FindObjectOfType<PlayerBase>();
        playInfo = GetComponent<PlayInfo>();
    }
    private void OnEnable()
    {
        save1.onClick.AddListener(SaveFile);
        startSave.onSave += SaveFileStart;
    }


    private void SaveFileStart()
    {
        gameObject.transform.parent.gameObject.SetActive(true);

        SaveData saveData = new();
        playerName = saveData.playerName;
        currentDay = timer.day;
        currentTime = timer.hour;
        PlayerData(player);
        currentTimeX = sunshine.Vec.x;
        currentTimeY = sunshine.Vec.y;
        currentTimeZ = sunshine.Vec.z;
        SaveItem();
        workbenchPositionX = setUpItem.transform.position.x;
        workbenchPositionY = setUpItem.transform.position.y;
        workbenchPositionZ = setUpItem.transform.position.z;
    }

    //플레이어 데이터 저장
    private void PlayerData(PlayerBase player)
    {
        int newHp = player.HP;
        playerHp = newHp;   // 새 점수를 현재 랭킹에 끼워 넣기
        playerPositionX = player.transform.position.x;
        playerPositionY = player.transform.position.y;
        playerPositionZ = player.transform.position.z;

        player.GetToolItem(toolTag, toolLevel);
        toolName = (int)toolTag;
    }
    //아이템 데이터 저장
    void SaveItem()
    {
        if (ItemManager.Instance.itemInventory.ItemTypeArray != null)
        {
            itemCount = ItemManager.Instance.itemInventory.ItemAmountArray;
            for (int i = 0; i < ItemManager.Instance.itemInventory.ItemTypeArray.Length; i++)
            {
                itemTypes[i] = (int)ItemManager.Instance.itemInventory.ItemTypeArray[i];
            }
        }
    }

    //세이브데이터에 저장(json)파일 만들기
    void SaveRankingData()
    {
        SaveData saveData = new();

        // 생성한 인스턴스에 데이터 기록
        saveData.playerName = playerName;
        saveData.playerHp = playerHp;
        saveData.playerPositionX = playerPositionX;
        saveData.playerPositionY = playerPositionY;
        saveData.playerPositionZ = playerPositionZ;

        saveData.currentToolItemTag = toolName;
        saveData.toolLevel = toolLevel;

        saveData.currentTimeX = currentTimeX;
        saveData.currentTimeY = currentTimeY;
        saveData.currentTimeZ = currentTimeZ;

        saveData.currentTime = currentTime;
        saveData.currentDay = currentDay;

        if (ItemManager.Instance.itemInventory.ItemTypeArray != null)
        {
            saveData.itemCount = ItemManager.Instance.itemInventory.ItemAmountArray;
            for (int i = 0; i < ItemManager.Instance.itemInventory.ItemTypeArray.Length; i++)
            {
                saveData.itemTypes[i] = (int)ItemManager.Instance.itemInventory.ItemTypeArray[i];
            }
        }

        saveData.workbenchPositionX = setUpItem.transform.position.x;
        saveData.workbenchPositionY = setUpItem.transform.position.y;
        saveData.workbenchPositionZ = setUpItem.transform.position.z;


        string json = JsonUtility.ToJson(saveData);     // saveData에 있는 내용을 json 양식으로 설정된 string으로 변경

        string path = $"{Application.dataPath}/Save/";  // 저장될 경로 구하기(에디터에서는 Assets 폴더)
        if (!Directory.Exists(path))                    // path에 저장된 폴더가 있는지 확인
        {
            Directory.CreateDirectory(path);            // 폴더가 없으면 그 폴더를 만든다.
        }

        string fullPath = $"{path}Save.json";           // 전체 경로 = 폴더 + 파일이름 + 파일확장자
        File.WriteAllText(fullPath, json);              // fullPath에 json내용 파일로 기록하기        
    }

    //세이브파일 만들까말까
    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";              // 경로
        string fullPath = $"{path}Save.json";                       // 전체 경로

        result = Directory.Exists(path) && File.Exists(fullPath);   // 폴더와 파일이 있는지 확인

        if (result)
        {
            // 폴더와 파일이 있으면 읽기
            string json = File.ReadAllText(fullPath);                   // 텍스트 파일 읽기
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);   // json문자열을 파싱해서 SaveData에 넣기
        }
        else
        {
            char temp = 'A';
            temp = (char)((byte)temp);
            playerName = $"{temp}{temp}{temp}";     // AAA,BBB,CCC,DDD,EEE
        }
        RefreshSet();     // 로딩이 되었으니 RankLines 갱신
        return result;
    }

    /// <summary>
    /// 랭크 라인 화면 업데이트용 함수
    /// </summary>
    void RefreshSet()
    {
        //플레이어
        PlayerBase player = FindObjectOfType<PlayerBase>();
        player.transform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);
        LoadHp?.Invoke(playerHp);
        onChangeTool?.Invoke((ToolItemTag)toolName, toolLevel);

        //아이템
        if (ItemManager.Instance.itemInventory.ItemTypeArray != null)
        {
            ItemManager.Instance.itemInventory.ItemAmountArray = itemCount;
            for (int i = 0; i < itemTypes.Length; i++)
            {
                ItemManager.Instance.itemInventory.ItemTypeArray[i] = (ItemType)itemTypes[i];
            }
        }

        //맵
        Quaternion rotation = Quaternion.Euler(currentTimeX, currentTimeY, currentTimeZ);
        onLoardRotate?.Invoke(rotation);
        //ui
        onLoardDay?.Invoke(currentDay);
        onLoardTime?.Invoke(currentTime);

        SaveFile();
    }

    private void SaveFile()
    {
        playInfo.SetData(currentDay, currentTime);
        SaveRankingData();  // 새로 저장하고 
        RefreshSet(); // UI 갱신

        Debug.Log("저장~~~~~~~~~");
    }
}*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileUI : MonoBehaviour
{
    // 세이브보드 버튼 관리용
    public Button saveButton;

    private void Awake()
    {
        saveButton = gameObject.transform.GetChild(1).GetComponent<Button>();
    }
    private void OnEnable()
    {
        saveButton.onClick.AddListener(OnSaveFile);
    }

    private void OnDisable()
    {
        saveButton.onClick.RemoveAllListeners();
    }

    public void OnSaveFile()
    {
        DataController.Instance.SaveGameData();
    }
}