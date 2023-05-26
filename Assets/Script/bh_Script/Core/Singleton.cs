using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance 
    {
        get
        {
            if (programPowerOn == false)
            {
                Debug.LogWarning($"{typeof(T).Name} 싱글톤은 이미 삭제되었습니다.");
                return null;
            }
            else if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject newObj = new GameObject();
                    newObj.name = typeof(T).Name;
                    instance = newObj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private const int disableSceneNumber = -1;
    private static int nowSceneNumber = disableSceneNumber;
    private static bool programPowerOn = true;
    protected bool initialized = false;

    void Awake() {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else 
        {
            if (instance != this) 
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
            PreInitialize();
            Initialize();
    }

    protected virtual void PreInitialize()
    {
        if (initialized == false) 
        {
            initialized = true;
            Scene active = SceneManager.GetActiveScene();
            nowSceneNumber = active.buildIndex;
        }
    }

    protected virtual void Initialize()
    {

    }

    void OnApplicationQuit() 
    {
        programPowerOn = false;
    }


}
