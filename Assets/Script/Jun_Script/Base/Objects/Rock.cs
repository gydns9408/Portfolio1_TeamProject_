using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : BasementObject
{
    // public GameObject DropRock;

    public Action<int> RockHp;

    Rigidbody rigid;
    SphereCollider sphere;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphere = GetComponent<SphereCollider>();
    }
    private void Start()
    {
        RockHp += RockObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pick"))
        {
            Debug.Log("GetStarted");

            // 횟수로 한다.
            objectHP--;

            float rock_Random = UnityEngine.Random.Range(0.0f, 1.0f);

            if (objectHP > 0)
            {
                Debug.Log(objectHP);
                GameObject obj = Instantiate(Effect);
                obj.transform.position = transform.position;
            }

            else if(objectHP <= 0)
            {
                Debug.Log(objectHP);
                GameObject obj = Instantiate(Meffect);
                obj.transform.position = transform.position;

                // 드랍 아이템 생성

                if (collision.gameObject.transform.GetChild(0).gameObject.activeSelf == true)
                {
                    if(rock_Random <= 0.75f)
                    {
                        RockDrop1();
                    }
                    else if(rock_Random <= 0.95f)
                    {
                        RockDrop2();
                    }
                    else
                    {
                        RockDrop3();
                    }
                }
                else if (collision.gameObject.transform.GetChild(1).gameObject.activeSelf == true)
                {
                    if(rock_Random <= 0.6f)
                    {
                        RockDrop1();
                    }
                    else if(rock_Random <= 0.85f)
                    {
                        RockDrop2();
                    }
                    else
                    {
                        RockDrop3();
                    }
                }
                else if (collision.gameObject.transform.GetChild(2).gameObject.activeSelf == true)
                {
                    if(rock_Random <= 0.45f)
                    {
                        RockDrop1();
                    }
                    else if(rock_Random <= 0.7f)
                    {
                        RockDrop2();
                    }
                    else
                    {
                        RockDrop3();
                    }
                }
                else
                {
                    Debug.Log("None");
                }

                objectHP = objectMaxHP;
            }
        }
        else if (collision.gameObject.CompareTag("RightHand"))
        {
            handObjectHp--;

            Debug.Log(handObjectHp);

            float Hand_Random = UnityEngine.Random.Range(0.0f, 1.0f);
            Debug.Log(Hand_Random);

            if (handObjectHp > 0)
            {
                GameObject obj = Instantiate(Effect);
                obj.transform.position = transform.position;
            }
            else if (handObjectHp <= 0)
            {
                GameObject obj = Instantiate(Meffect);
                obj.transform.position = transform.position;

                float a_Hand = 0.25f;
                float b_Hand = 0.75f;
                float c_Hand = 1.0f;
                if (Hand_Random <= a_Hand)
                {
                    Debug.Log("None");
                }
                else if (Hand_Random <= b_Hand)
                {
                    Hand_Drop_Rock1();
                }
                else if (Hand_Random <= c_Hand)
                {
                    Hand_Drop_Rock2();
                }
                //else if (Hand_Random <= d_Hand)
                //{
                //    // 플레이어의 체력 증가
                //    Hand_HpCare();
                //}
                else
                {
                    Debug.Log("ERROR");
                }
                handObjectHp = handObjectMaxHp;
            }
        }
        else
        {
            Debug.Log("This Tool can not be used");
        }
    }

    void RockObject(int Hp)
    {
        Hp = objectHP;
    }
}
