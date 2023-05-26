using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{

    public Transform target;
    public float speed = 1.0f;
    Vector3 offset;
    float lenght;

    // 시네머신 카메라용 컴포넌트
    CinemachineBrain brain;
    public CinemachineBrain Brain => brain; // 사망 카메라 전달용 프로퍼티
    PlayerBase player;                      // 플레이어 사망 델리게이트 받기용

    private void Awake()
    {
        brain = GetComponent<CinemachineBrain>();   // 시네머신 브레인
        player = FindObjectOfType<PlayerBase>();    // 플레이어 찾기
    }

    private void Start()
    {
        brain.enabled = false;  // 시네머신 비활성화해서 기존 카메라 시점으로 게임 진행
        player.onDie += OnCineMachine;  // 사망시 시네머신 함수 실행
        StartCoroutine(DDorutine());
    }

    IEnumerator DDorutine()
    {
        yield return new WaitForSeconds(0.05f);
        if (DataController.Instance.WasSaved == false)
        {
            PreInitialize();
        }
        else
        {
            Initialize();
        }
    }

    private void PreInitialize()
    {
        Vector3 start = new Vector3(0, 3.46f, -3.13f);
        transform.position = start;
        if (target == null)
        {
            PlayerBase player = FindObjectOfType<PlayerBase>();
            target = player.transform.GetChild(2);
        }

        offset = transform.position - target.position;
        lenght = offset.magnitude;
    }

    private void Initialize()
    {
        Vector3 start = DataController.Instance.gameData.playerPosition + new Vector3(0, 3.46f, -3.13f);
        transform.position = start;
        if (target == null)
        {
            PlayerBase player = FindObjectOfType<PlayerBase>();
            target = player.transform.GetChild(2);
        }
        offset = transform.position - target.position;
        lenght = offset.magnitude;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(     // 호를 그리며 움직이게 만들기
            transform.position,                 // 현재 위치에서 
            target.position + Quaternion.LookRotation(target.forward) * offset, // offset만큼 떨어진 위치로(회전 적용됨)
            1.0f);       // Time.fixedDeltaTime * speed만큼 보간

        transform.LookAt(target);               // 카메라가 목표지점 바라보기

        // target에서 카메라로 나가는 레이
        Ray ray = new Ray(target.position, transform.position - target.position);
        if (Physics.Raycast(ray, out RaycastHit hit, lenght))  // 충돌 체크
        {
            transform.position = hit.point;                     // 충돌하면 충돌한 위치로 카메라 옮김
        }
    }


    private void OnCineMachine()
    {
        brain.enabled = true;   // 브레인 실행
    }
}
