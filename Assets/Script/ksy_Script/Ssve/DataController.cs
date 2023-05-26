using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : MonoBehaviour
{
    public bool WasSaved = false;

    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }

    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    public string GameDataFileName = "Save.json";

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
            }
            return _gameData;
        }
    }

    public void LoadGameData()
    {
        string filePath = $"{Application.dataPath}/Save/Save.json";
        if (File.Exists(filePath))
        {
            WasSaved = true;
            Debug.Log("�ҷ����� ����");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("���ο� ���� ����");
            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = $"{Application.dataPath}/Save/Save.json";

        // ���� ����� ���丮�� ������ ���丮�� �����մϴ�.
        string directoryPath = Path.GetDirectoryName(filePath);
        Directory.CreateDirectory(directoryPath);

        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("����Ϸ�");
    }

    public void DeleteSaveFile()
    {
        WasSaved = false;
        string filePath = $"{Application.dataPath}/Save/Save.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("���̺� ���� ���� �Ϸ�");
        }
        else
        {
            Debug.Log("������ ���̺� ������ �����ϴ�");
        }
    }
}