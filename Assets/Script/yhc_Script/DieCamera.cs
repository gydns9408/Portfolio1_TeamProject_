using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieCamera : MonoBehaviour
{
    PlayerBase player;
    CinemachineVirtualCamera vCamera;
    CinemachineDollyCart dollyCart;

    private void Awake()
    {
        player = FindObjectOfType<PlayerBase>();
        vCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        vCamera.LookAt = player.transform;
        dollyCart = GetComponentInChildren<CinemachineDollyCart>();
    }
        
    private void Start()
    {
        vCamera.Follow = dollyCart.transform;   // 플레이어
        player.onDie += ProduceStart;
    }

    private void ProduceStart()
    {
        Debug.Log("카메라 작동");
        transform.position = player.transform.position;     // 시네머신 구성품들 위치 플레이어쪽으로 이동
        dollyCart.transform.position = transform.position;
        vCamera.transform.position = transform.position;
        vCamera.Priority = 100;
        dollyCart.m_Speed = 3;
    }
}
