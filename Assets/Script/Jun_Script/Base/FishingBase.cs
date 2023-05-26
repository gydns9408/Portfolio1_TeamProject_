using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.ProBuilder;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class FishingBase : MonoBehaviour
{
    // 이펙트
    public GameObject fishingEffect;        // 물에 닿았을 때의 이펙트

    public Transform target;                // 타겟(종점 좌표)

    //public float flightTime = 2.0f;         // 체공 시간
    //public float speedRate = 1.0f;          // 허공 시간을 기준으로 한 이동속도의 배율
    //private const float gravity = -9.8f;    // 중력

    private Transform endPos;                 // 타겟의 위치
    private Vector3 startPos;               // 생성 위치

    // 낚시할때의 대기시간
    public float waitT = 5.0f;              // 낚시후의 딜레이 시간

    Rigidbody rigid;

    private GameObject desTarget;


    private bool isTimeCheck = false;

    private void Start()
    {
        endPos = target.transform;
        if (target == null)
        {
            Player player = FindObjectOfType<Player>();
            target = player.transform;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FisingRod"))
        {
            GameObject obj = Instantiate(fishingEffect);
            obj.transform.position = transform.position;

            float fish_Random = UnityEngine.Random.Range(0.0f, 1.0f);

            isTimeCheck = false;

            if (isTimeCheck == false)
            {
                Debug.Log("Step1");

                if (other.gameObject.transform.GetChild(0).gameObject.activeSelf == true)
                {
                    if(fish_Random<= 0.75f)
                    {
                        Invoke("FishDrop1", waitT);
                    }
                    else if(fish_Random <= 0.95f)
                    {
                        Invoke("FishDrop2", waitT);
                    }
                    else
                    {
                        Invoke("FishDrop3", waitT);
                    }
                }

                else if (other.gameObject.transform.GetChild(1).gameObject.activeSelf == true)
                {
                    if(fish_Random <= 0.6f)
                    {
                        Invoke("FishDrop1", waitT);
                    }
                    else if(fish_Random <= 0.85f)
                    {
                        Invoke("FishDrop2", waitT);
                    }
                    else
                    {
                        Invoke("FishDrop3", waitT);
                    }
                }

                else if (other.gameObject.transform.GetChild(2).gameObject.activeSelf == true)
                {
                    if(fish_Random <= 0.45f)
                    {
                        Invoke("FishDrop1", waitT);
                    }
                    else if(fish_Random <= 0.7f)
                    {
                        Invoke("FishDrop2", waitT);
                    }
                    else
                    {
                        Invoke("FishDrop3", waitT);
                    }
                }
            }
            isTimeCheck = true;
            Debug.Log("Step2");
        }
        else
        {
            Debug.Log("This Tool can not be used");
        }
    }

    public void FishDrop1()
    {
        Debug.Log("Alpha");
        GameObject fobj = ItemManager.Instance.GetObject(ItemType.Gazami);
        fobj.transform.position = transform.position;
        desTarget = fobj;

        startPos = fobj.transform.position;
        Vector3 disVec = (endPos.position - startPos) + new Vector3(0 , 5 , 0) + Vector3.right*2;
        rigid = fobj.GetComponent<Rigidbody>();
        rigid.AddForce(disVec*0.04f , ForceMode.Impulse);

        Debug.Log("Step3-1");
    }

    private void FishDrop2()
    {
        GameObject fobj = ItemManager.Instance.GetObject(ItemType.Galchi);
        fobj.transform.position = transform.position;
        desTarget = fobj;

        startPos = fobj.transform.position;
        Vector3 disVec = (endPos.position - startPos) + new Vector3(0, 5, 0) + Vector3.right*2;
        rigid = fobj.GetComponent<Rigidbody>();
        rigid.AddForce(disVec*0.04f , ForceMode.Impulse);

        Debug.Log("Step3-2");
    }

    private void FishDrop3()
    {
        GameObject fobj = ItemManager.Instance.GetObject(ItemType.Shark);
        fobj.transform.position = transform.position;
        desTarget = fobj;

        startPos = fobj.transform.position;
        Vector3 disVec = (endPos.position - startPos) + new Vector3(0, 5, 0) + Vector3.right*6;
        rigid = fobj.GetComponent<Rigidbody>();
        rigid.AddForce(disVec*0.4f , ForceMode.Impulse);

        //Invoke("FishFalse", 6.0f);
        Debug.Log("Step3-3");
    }

    private void FishFalse()
    {
        desTarget.SetActive(false);
    }
}