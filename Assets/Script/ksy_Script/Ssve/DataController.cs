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
            Debug.Log("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = $"{Application.dataPath}/Save/Save.json";

        // 파일 경로의 디렉토리가 없으면 디렉토리를 생성합니다.
        string directoryPath = Path.GetDirectoryName(filePath);
        Directory.CreateDirectory(directoryPath);

        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("저장완료");
    }

    public void DeleteSaveFile()
    {
        WasSaved = false;
        string filePath = $"{Application.dataPath}/Save/Save.json";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("세이브 파일 삭제 완료");
        }
        else
        {
            Debug.Log("삭제할 세이브 파일이 없습니다");
        }
    }
}