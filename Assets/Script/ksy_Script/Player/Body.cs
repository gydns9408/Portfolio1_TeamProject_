using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerBase;

public class Body : MonoBehaviour
{
    PlayerBase player;
    private void Awake()
    {
        player = FindObjectOfType<PlayerBase>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Workbench") && ItemManager.Instance.SetUpAItem.gameObject.activeSelf == true)
        {
            player.ifCraft = true;
        }
        // �� ������Ʈ�� Ʈ���Ű� ����� ��
        player.IsdEqualState[0] = true;
        if (other.gameObject.CompareTag("Tree"))
        {
            player.isAction = true;
            player.State = playerState.TreeFelling;
            if (player.IsdEqualState[2] || player.IsdEqualState[3] || player.IsdEqualState[4] == true)
            {
                player.isAction = false;
                Debug.Log("���Ұ� ������");
            }

            //isTreeFelling = true;
        }
        else if (other.gameObject.CompareTag("Flower"))
        {
            player.isAction = true;
            player.State = playerState.Gathering;
            if (player.IsdEqualState[1] || player.IsdEqualState[3] || player.IsdEqualState[4] == true)
            {
                player.isAction = false;
                Debug.Log("���Ұ� ������");
            }
        }
        else if (other.gameObject.CompareTag("Rock"))
        {
            player.isAction = true;
            player.State = playerState.Mining;
            if (player.IsdEqualState[1] || player.IsdEqualState[2] || player.IsdEqualState[4] == true)
            {
                player.isAction = false;
                Debug.Log("���Ұ� ������");
            }
        }
        else if (other.gameObject.CompareTag("Ocean"))
        {
            player.isAction = true;
            player.State = playerState.Fishing;
            if (player.IsdEqualState[1] || player.IsdEqualState[2] || player.IsdEqualState[3] == true)
            {
                player.isAction = false;
                Debug.Log("���Ұ� ������");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Workbench") || ItemManager.Instance.SetUpAItem.gameObject.activeSelf == true)
        {
            player.ifCraft = false;
        }
        if (other.gameObject.CompareTag("Tree")
           || other.gameObject.CompareTag("Flower")
           || other.gameObject.CompareTag("Rock")
           || other.gameObject.CompareTag("Ocean"))
        {
            player.isAction = false;
        }
    }
}
