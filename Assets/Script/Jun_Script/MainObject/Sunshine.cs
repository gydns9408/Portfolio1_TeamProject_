using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;


public class Sunshine : MonoBehaviour
{
    //public delegate void ReSpawnObject();                  // 스폰을 위해 태양의 회전과 밤낮을 델리게이트로
    //public static ReSpawnObject OnRespawn;                 // 이 클래스내의 함수를 OnRespawn변수로 참조

    public Action OnRspawn;

    public Quaternion Vec
    {
        get => vec;
        set
        {
            if(vec != value)
            {
                vec = value;
                DayTimeDeli?.Invoke(vec);
            }
        }
    }

    public Action<Quaternion> DayTimeDeli;
    public Action HourChange;

    // CubemapTransition 관련 변수 --------------------------------------------------------
    public Material skybox;
    float alpha;
    public float beta = 0.5f;
    private float gamma = 0.5f;
    public float maxAlpha = 0.6f;

    // 낮밤 확인 변수
    public bool isNight;

    // Sunshine의 회전값을 나타내는 변수 (Update)--------------------------------------------
    public float vecT = 0.5f;    // 6분경과 : 0.3333f
    float t;
    float round;

    // 기타-------------------------------------------------------------------------------
    Quaternion vec;
    SaveBoardUI pauseMenu;
    // fog
    private float morningFog;   // 아침의 안개량

    //15도마다 델리게이트를 송신하기 위한 체크용 변수.
    int currentRound = 0;


    // Sunshine의 회전값을 나타내는 변수 (FixedUpdate) --------------------------------------
    float rec = 1.0f;
    public float rotationAmount;


    private void Awake()
    {
        pauseMenu = FindObjectOfType<SaveBoardUI>();
    }

    private void Start()
    {
        rotationAmount = 0.0f;
        isNight = false;
        alpha = 0.0f;

        //HourChange += RoundTime;                        // 여기서 선샤인값을 받는다.
        RenderSettings.fogDensity = morningFog;

        pauseMenu.updateData += SetData;
        if (DataController.Instance.WasSaved == false)
        {
            PreInitialize();
        }
        else
        {
            Initialize();
        }
    }

    private void Update()
    {
        OnRspawn?.Invoke();
        SunRotate();
        SetFog();
    }

    private void FixedUpdate()
    {
        RotateFixed();
    }

    public void RotateFixed()
    {
        rotationAmount += rec * Time.fixedDeltaTime;
        Quaternion deltaQua = Quaternion.Euler(rotationAmount, 0, 0);
        transform.rotation = deltaQua;

        if ((int)(rotationAmount % 15) == 0 && currentRound != (int)rotationAmount && (int)rotationAmount != 360)
        {
            //델리게이트
            currentRound = (int)rotationAmount;
            Debug.Log("SunRotate에서 15도 초과로 인한 델리게이트 발생");
            HourChange?.Invoke();
        }
    }
    public void SunRotate()
    {
        // [1] 목표 : 12분에 0 ~ 180도만큼 회전 -> 12분에 180 ~ 360도 회전 : 180/720 = 1/4 : 0.25
        // transform.Rotate(new Vector3(forTimeInGame * t  , 0f , 0f)); // light의 회전

        t += Time.deltaTime;
        round = vecT * t;
        vec = Quaternion.Euler(round, 0, 0);

        // transform.rotation = vec;  // 1초에 0.25도 만큼 회전
                                   // Debug.Log(transform.rotation.x);
        DayTimeDeli?.Invoke(vec);
        // 로테이션값을 시간처럼 시각화하기
        if (round > 360f)
        {
            t = 0f;
        }
    }

    public void SetFog()
    {
        Vector3 eulerAngle = transform.rotation.eulerAngles;
        //Debug.Log(eulerAngle);
        if (eulerAngle.x <= 180.0f && eulerAngle.x >= 0.0f) //  0 <= x <= 170
        {
            RenderSettings.fogDensity = morningFog;             // 아침의 fog량은 0.0001로 고정.
            isNight = false;                    // 낮이 된다.
        }

        else if ((eulerAngle.x >180.0f && eulerAngle.x <=360.0f)||(eulerAngle.x < 0.0f))     // Light의 x값이 170보다 커지면
        {
            isNight = true;                     // 밤
        }



        if (isNight)                            // 밤이 되면
        {
            RenderSettings.fogDensity += 0.000004f;
            if (RenderSettings.fogDensity >= 0.1f) 
            {
                RenderSettings.fogDensity = 0.1f;
            }

            if (alpha >= 0.6f && alpha < 0.8f)
            {
                alpha += beta* gamma * Time.deltaTime;
                skybox.SetFloat("_CubemapTransition", alpha);
            }
            else
            {
                alpha = 0.8f;
                skybox.SetFloat("_CubemapTransition", alpha);
            }
        }

        else
        {
            if (alpha < 0.6f && alpha >= 0.0f)      // -0.1~
            {
                alpha += beta * gamma * Time.deltaTime;
                skybox.SetFloat("_CubemapTransition", alpha);   // CubemapTrasition의 수치가 alpha가 된다. : 0 ~ 0.6
            }

            else if (alpha >= maxAlpha && alpha < 0.7f)
            {
                alpha = maxAlpha;                               // 0.6
            }
            else
            {
                alpha = 0.0f;
            }
        }
    }

    public void SetData()
    {
        //DataController.Instance.gameData.currentSunRotate = round;
        //DataController.Instance.gameData.currentRotateTime = t;
        DataController.Instance.gameData.currentSunRotate = rotationAmount;
        DataController.Instance.gameData.currentIsNight = isNight;
        DataController.Instance.gameData.cubemap = alpha;
        DataController.Instance.gameData.fogDens = RenderSettings.fogDensity;
    }
    private void PreInitialize()
    {
        //t = 0.0f;
        //round = 0.0f;
        rotationAmount = 0.0f;
        isNight = false;
        alpha = 0.0f;
        RenderSettings.fogDensity = morningFog;
    }

    private void Initialize()
    {
        //round = DataController.Instance.gameData.currentSunRotate;
        //t = DataController.Instance.gameData.currentRotateTime;
        //vec = Quaternion.Euler(round, 0, 0);
        rotationAmount = DataController.Instance.gameData.currentSunRotate;
        isNight = DataController.Instance.gameData.currentIsNight;
        alpha = DataController.Instance.gameData.cubemap;
        RenderSettings.fogDensity = DataController.Instance.gameData.fogDens;
    }
}
