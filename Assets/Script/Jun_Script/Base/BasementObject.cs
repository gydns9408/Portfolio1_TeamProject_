using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BasementObject : MonoBehaviour
{
    // 돌 , 꽃 , 나무가 공통으로 가지는 Base
    // 체력 , effect
    public int objectMaxHP = 3;
    public int objectHP = 3;

    public int handObjectMaxHp = 5;
    public int handObjectHp = 5;

    private int myNumber;
    Transform childPlace;
    int ObjectHP
    {
        get => objectHP;
        set
        {
            if(objectHP != value)
            {
                objectHP = value;
                playerHpDel?.Invoke(objectHP);
            }
        }
    }

    public Action<int> playerHpDel;

    public Action<int> playerHpGift;        // 델리게이트를 주기위한 차선책 1 (return 값으로 나오지않고 int a값만 받아오면 가능할듯...)

    Func<int, float, int> gift;             // Func으로 int형으로 변경후 return 값을 받아오는 형식

    public GameObject Effect;   // 치집 , 채굴시의 이펙트
    public GameObject Meffect;

    public float delayTime = 4.0f;

    private GameObject target;

    private void Start()
    {

    }

    public void FlowerDrop1()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Strawberry); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }

    public void FlowerDrop2()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Avocado); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }

    public void FlowerDrop3()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Peanut); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }


    public void TreeDrop1()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Firewood); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position;
        target = obj;
    }


    public void TreeDrop2()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.FirewoodX3); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position;
        target = obj;
    }

    public void TreeDrop3()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.FirewoodX5); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position;
        target = obj;
    }

    public void RockDrop1()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Stone); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position + Vector3.forward * 2 + Vector3.right * 0.3f;
        target = obj;
    }

    public void RockDrop2()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Iron); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position + Vector3.forward * 2 + Vector3.right * 0.3f;
        target = obj;
    }

    public void RockDrop3()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Gold); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        obj.transform.position = transform.position + Vector3.forward * 2 + Vector3.right * 0.3f;
        target = obj;
    }

    private void TimeCount()
    {
        Debug.Log(target);
        target.SetActive(false);
    }
    // 맨손 Tree--------------------------------------------------------------------------------------

    public void Hand_Drop_Tree1()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Firewood);
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }
    public void Hand_Drop_Tree2()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.FirewoodX3);
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }
    public void Hand_Drop_Tree3()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.FirewoodX5);
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }
    // 맨손 Rock--------------------------------------------------------------------------------------

    public void Hand_Drop_Rock1()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Stone);
        obj.transform.position = transform.position + Vector3.forward * 2 + Vector3.right*0.3f;
        target = obj;
    }
    public void Hand_Drop_Rock2()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Iron);
        obj.transform.position = transform.position + Vector3.forward * 2 + Vector3.right * 0.3f;
        target = obj;
    }
    public void Hand_Drop_Rock3()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Gold);
        obj.transform.position = transform.position + Vector3.forward * 2 + Vector3.right * 0.3f;
        target = obj;
    }
    // 맨손 Flower------------------------------------------------------------------------------------

    public void Hand_Drop_Flower1()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Strawberry);
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }

    public void Hand_Drop_Flower2()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Avocado);
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }

    public void Hand_Drop_Flower3()
    {
        GameObject obj = ItemManager.Instance.GetObject(ItemType.Peanut);
        obj.transform.position = transform.position + Vector3.up;
        target = obj;
    }

    // 맨손 체력회복-------------------------------------------------------------------------------------
    public void Hand_HpCare()
    {
        //playerHpGift = HealingHp();

        // 델리게이트안에서 계산....................................(1)
        // a의값만 playerHpGift(a);로 플레이어 쪽에서 입력하면 될듯...?
        // playerHpGift = (int a) => { int sum = a + (int)(a*0.25f);};         // 현재 체력 + 현재체력의 25%
        gift = (int a, float b) => { b = a * 0.25f; int sum = a + (int)b; return sum; };
    }


    int HealingHp(int a)
    {
        int playerHeal = a + (int)(a*0.25f);
        return playerHeal;
    }
}
