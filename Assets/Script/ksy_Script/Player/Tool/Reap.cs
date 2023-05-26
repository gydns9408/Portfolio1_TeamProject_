using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reap : MonoBehaviour
{
    public Action<int> UsingTool;
    public Collider ReapCollider;
    int useToolHp;

    private void Start()
    {
        ReapCollider = GetComponent<Collider>();
        ReapCollider.enabled = false;
    }
    public void OnCangeReapLevel()
    {
        if (ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Sickle) > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            transform.GetChild(ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Sickle) - 1).gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }
    }

    private int UsingToolReap(int hp)
    {
        int toolLevel = ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Sickle);
        switch (toolLevel)
        {
            case 1:
                hp = -20;
                break;
            case 2:
                hp = -14;
                break;
            case 3:
                hp = -11;
                break;
        }
        UsingTool?.Invoke(hp);
        Debug.Log(hp);
        return hp;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Flower"))
        {
            UsingToolReap(useToolHp);
        }
    }
}
