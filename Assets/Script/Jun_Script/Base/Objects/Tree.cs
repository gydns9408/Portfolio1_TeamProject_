using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class Tree : PlaneBase
{
    public Action<int> TreeHp;

    Sunshine sun;
    private bool isDisTree;     // 나무가 사라졌는지 확인하기위한 변수
    SaveBoardUI pauseMenu;

    private void Awake()
    {
        pauseMenu = FindObjectOfType<SaveBoardUI>();
        sun = FindObjectOfType<Sunshine>();
    }

    private void Start()
    {
        sun.OnRspawn += Respawn;      // Onrespawn의 낮밤이 실행될때 respawn실행(sunshine에서 update에 넣었기 때문에 Update를 우회하여 실행)
        TreeHp += TreeObject;
        isDisTree = false;

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

    private void Respawn()
    {
        // Debug.Log(gameObject);
        if (isDisTree)
        {
            float qua = sun.transform.rotation.x;
            if ((qua >= -0.0004f && qua <= 0.0f) || (qua <= 0.0004f && qua >= 0.0f))
            {
                gameObject.SetActive(true);
                isDisTree = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Axe"))
        {
            Debug.Log("AXE");
            objectHP--;
            Debug.Log($"First : {objectHP}");

            float tree_Random = UnityEngine.Random.Range(0.0f, 1.0f);

            if (objectHP > 0)
            {
                Debug.Log("TreeStart");
                GameObject obj = Instantiate(Effect);
                obj.transform.position = transform.position;
            }

            else if (objectHP == 0)
            {
                Debug.Log($"Second : {objectHP}");
                GameObject obj = Instantiate(Meffect);
                obj.transform.position = transform.position;

                gameObject.SetActive(false);
                isDisTree = true;
                Debug.Log("do");

                if (collision.gameObject.transform.GetChild(0).gameObject.activeSelf == true)
                {
                    if(tree_Random <= 0.75f)
                    {
                        TreeDrop1();
                    }
                    else if(tree_Random <= 0.95f)
                    {
                        TreeDrop2();
                    }
                    else
                    {
                        TreeDrop3();
                    }
                }
                else if (collision.gameObject.transform.GetChild(1).gameObject.activeSelf == true)
                {
                    if(tree_Random <= 0.6f)
                    {
                        TreeDrop1();
                    }
                    else if(tree_Random <= 0.85f)
                    {

                        TreeDrop2();
                    }
                    else
                    {
                        TreeDrop3();
                    }
                }
                else if (collision.gameObject.transform.GetChild(2).gameObject.activeSelf == true)
                {
                    if(tree_Random <= 0.45f)
                    {
                        TreeDrop1();
                    }
                    else if(tree_Random <= 0.7f)
                    {
                        TreeDrop2();
                    }
                    else
                    {
                        TreeDrop3();
                    }
                }
                else
                {
                    Debug.Log("None");
                }
                objectHP = objectMaxHP;                         // 체력이0이되면서 아이템이 생성되며 오브젝트의 체력을 max체력으로 돌려준다.
            }
        }

        else if (collision.gameObject.CompareTag("RightHand"))
        {
            Debug.Log("RightHand");
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

                gameObject.SetActive(false);
                isDisTree = true;
                float a_Hand = 0.25f;
                float b_Hand = 0.75f;
                float c_Hand = 1.00f;
                // float d_Hand = 0.00f;
                if (Hand_Random <= a_Hand)
                {
                    Debug.Log("None");
                }
                else if (Hand_Random <= b_Hand)
                {
                    Hand_Drop_Tree1();
                }
                else if (Hand_Random <= c_Hand)
                {
                    Hand_Drop_Tree2();
                }
                //else if (Hand_Random <= d_Hand)
                //{
                //    // 플레이어의 체력 증가
                //    Hand_Drop_Tree3();
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
    void TreeObject(int Hp)
    {
        Hp = objectHP;
    }

    public void SetData()
    {
        DataController.Instance.gameData.treeIsTree = isDisTree;
    }
    private void PreInitialize()
    {
        isDisTree = false;
    }

    private void Initialize()
    {
        isDisTree = DataController.Instance.gameData.treeIsTree;
    }
}
