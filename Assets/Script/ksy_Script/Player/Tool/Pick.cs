using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    public Action<int> UsingTool;
    public Collider pickCollider;
    int useToolHp;

    private void Start()
    {
        pickCollider = GetComponent<Collider>();
        pickCollider.enabled = false;
    }
    public void OnCangePickLevel()
    {
        if (ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Pickaxe) > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            transform.GetChild(ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Pickaxe) - 1).gameObject.SetActive(true);
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

    private int UsingToolPick(int hp)
    {
        int toolLevel = ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Pickaxe);
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
        if (other.gameObject.CompareTag("Rock"))
        {
            UsingToolPick(useToolHp);
        }

    }
}
