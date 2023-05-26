/*public class SaveFileUI : MonoBehaviour
{
    Button save1;

    PauseMenu startSave;

    PlayInfo playInfo;
    SetUpItem setUpItem;
    Sunshine sunshine;
    Timer timer;

    PlayerBase player;
    string playerName;         //�̸�
    int playerHp;
    float playerPositionX;      //��ġ
    float playerPositionY;
    float playerPositionZ;

    int[] itemCount = null;             // ����
    int[] itemTypes = new int[ItemManager.Instance.itemInventory.ItemTypeArray.Length];        // ����

    float workbenchPositionX;   //���۴� ��ġ
    float workbenchPositionY;
    float workbenchPositionZ;

    ToolItemTag toolTag;        // ������ �������� �̸�
    int toolName;
    int toolLevel;         // ������ �������� ����

    int currentDay;         // ��¥ ���� �迭
    int currentTime;

    float currentTimeX;     // �����̼� ������ ���� �ð� ����
    float currentTimeY;     // �����̼� ������ ���� �ð� ����
    float currentTimeZ;     // �����̼� ������ ���� �ð� ����

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

    //�÷��̾� ������ ����
    private void PlayerData(PlayerBase player)
    {
        int newHp = player.HP;
        playerHp = newHp;   // �� ������ ���� ��ŷ�� ���� �ֱ�
        playerPositionX = player.transform.position.x;
        playerPositionY = player.transform.position.y;
        playerPositionZ = player.transform.position.z;

        player.GetToolItem(toolTag, toolLevel);
        toolName = (int)toolTag;
    }
    //������ ������ ����
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

    //���̺굥���Ϳ� ����(json)���� �����
    void SaveRankingData()
    {
        SaveData saveData = new();

        // ������ �ν��Ͻ��� ������ ���
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


        string json = JsonUtility.ToJson(saveData);     // saveData�� �ִ� ������ json ������� ������ string���� ����

        string path = $"{Application.dataPath}/Save/";  // ����� ��� ���ϱ�(�����Ϳ����� Assets ����)
        if (!Directory.Exists(path))                    // path�� ����� ������ �ִ��� Ȯ��
        {
            Directory.CreateDirectory(path);            // ������ ������ �� ������ �����.
        }

        string fullPath = $"{path}Save.json";           // ��ü ��� = ���� + �����̸� + ����Ȯ����
        File.WriteAllText(fullPath, json);              // fullPath�� json���� ���Ϸ� ����ϱ�        
    }

    //���̺����� ������
    bool LoadRankingData()
    {
        bool result = false;

        string path = $"{Application.dataPath}/Save/";              // ���
        string fullPath = $"{path}Save.json";                       // ��ü ���

        result = Directory.Exists(path) && File.Exists(fullPath);   // ������ ������ �ִ��� Ȯ��

        if (result)
        {
            // ������ ������ ������ �б�
            string json = File.ReadAllText(fullPath);                   // �ؽ�Ʈ ���� �б�
            SaveData loadData = JsonUtility.FromJson<SaveData>(json);   // json���ڿ��� �Ľ��ؼ� SaveData�� �ֱ�
        }
        else
        {
            char temp = 'A';
            temp = (char)((byte)temp);
            playerName = $"{temp}{temp}{temp}";     // AAA,BBB,CCC,DDD,EEE
        }
        RefreshSet();     // �ε��� �Ǿ����� RankLines ����
        return result;
    }

    /// <summary>
    /// ��ũ ���� ȭ�� ������Ʈ�� �Լ�
    /// </summary>
    void RefreshSet()
    {
        //�÷��̾�
        PlayerBase player = FindObjectOfType<PlayerBase>();
        player.transform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);
        LoadHp?.Invoke(playerHp);
        onChangeTool?.Invoke((ToolItemTag)toolName, toolLevel);

        //������
        if (ItemManager.Instance.itemInventory.ItemTypeArray != null)
        {
            ItemManager.Instance.itemInventory.ItemAmountArray = itemCount;
            for (int i = 0; i < itemTypes.Length; i++)
            {
                ItemManager.Instance.itemInventory.ItemTypeArray[i] = (ItemType)itemTypes[i];
            }
        }

        //��
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
        SaveRankingData();  // ���� �����ϰ� 
        RefreshSet(); // UI ����

        Debug.Log("����~~~~~~~~~");
    }
}*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileUI : MonoBehaviour
{
    // ���̺꺸�� ��ư ������
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