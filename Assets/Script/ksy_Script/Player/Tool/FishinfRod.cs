using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishinfRod : MonoBehaviour
{
    public Action<int> UsingTool;
    public Collider fishigrodCollider;
    int useToolHp;

    private void Start()
    {
        fishigrodCollider = GetComponent<Collider>();
        fishigrodCollider.enabled = false;
    }

    public void OnCangeFishinfRodlLevel()
    {
        if (ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Fishingrod) > 0)
        {
            for (int i = 0; i < 3; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            transform.GetChild(ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Fishingrod) - 1).gameObject.SetActive(true);
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

    private int UsingToolFishingRod(int hp)
    {
        int toolLevel = ItemManager.Instance.itemInventory.GetEquipToolLevel(ToolItemTag.Fishingrod);
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Ocean"))
        {
            UsingToolFishingRod(useToolHp);
        }
    }
}
