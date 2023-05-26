using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RightHand : MonoBehaviour
{
    PlayerBase player;
    public Action<int> UsingTool;
    public Collider rHandCollider;
    int useToolHp = -17;

    private void Start()
    {
        rHandCollider = GetComponent<Collider>();

    }


    private void Awake()
    {
        player = FindObjectOfType<PlayerBase>();
    }

    private void UsingRhand()
    {
        UsingTool?.Invoke(useToolHp);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("Ocean") || collision.gameObject.CompareTag("Rock") || collision.gameObject.CompareTag("Flower"))
        {
            UsingRhand();
            StartCoroutine(SetUpTrigger());
        }
    }

    IEnumerator SetUpTrigger()
    {
        yield return null;
        rHandCollider.isTrigger = true;
        yield return new WaitForSeconds(0.7f);
        rHandCollider.isTrigger = false;
    }
}