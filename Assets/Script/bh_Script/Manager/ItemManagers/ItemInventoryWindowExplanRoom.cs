using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class ItemInventoryWindowExplanRoom : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI _ItemName;
    public TextMeshProUGUI _itemName
    {
        get => _ItemName;
        set => _ItemName = value;
    }
    TextMeshProUGUI _ItemExplan;
    public TextMeshProUGUI _itemExplan
    {
        get => _ItemExplan;
        set => _ItemExplan = value;
    }
    TextMeshProUGUI _ItemTag;
    public TextMeshProUGUI _itemTag
    {
        get => _ItemTag;
        set => _ItemTag = value;
    }
    Button _ItemUseButton;
    public Button _itemUseButton
    {
        get => _ItemUseButton;
        set => _ItemUseButton = value;
    }
    Button _ItemDumpButton;
    public Button _itemDumpButton
    {
        get => _ItemDumpButton;
        set => _ItemDumpButton = value;
    }

    public Action<int> onChangeHp;
    public Action<ToolItemTag, int> onChangeTool;

    ItemInventoryWindow itemInventoryWindow;
    public ItemInventoryWindow ItemInventoryWindow_p
    {
        set => itemInventoryWindow = value;
    }

    CraftingWindow craftingWindow;
    public CraftingWindow CraftingsWindow
    {
        get => craftingWindow;
        set => craftingWindow = value;
    }

    void Awake()
    {
        Transform child1 = transform.GetChild(0);
        _itemName = child1.GetComponent<TextMeshProUGUI>();
        Transform child2 = transform.GetChild(1);
        _itemExplan = child2.GetComponent<TextMeshProUGUI>();
        Transform child3 = transform.GetChild(2);
        _itemTag = child3.GetComponent<TextMeshProUGUI>();
        Transform child4 = transform.GetChild(3);
        _itemUseButton = child4.GetComponent<Button>();
        Transform child5 = transform.GetChild(4);
        _itemDumpButton = child5.GetComponent<Button>();
        initialize();
    }

    void Start()
    {
        //itemInventoryWindow = FindObjectOfType<ItemInventoryWindow>();
        //_itemUseButton.onClick.AddListener(ItemUse);
        //_itemDumpButton.onClick.AddListener(ItemDump);
    }

    void OnDisable()
    {
        initialize();
    }

    public void initialize()
    {
        _itemName.text = string.Empty;
        _itemExplan.text = string.Empty;
        _itemTag.text = string.Empty;
        _itemUseButton.gameObject.SetActive(false);
        _itemDumpButton.gameObject.SetActive(false);
    }

    public void ItemUse()
    {
        if (ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]].Tag == ItemTag.Food)
        {
            ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] -= 1;
            if (((FoodItemData)ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]]).ItemsAffectStatus == AffectStatus.Hp)
            {
                onChangeHp?.Invoke(((FoodItemData)ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]]).AmountOfHungerRecovery);
            }
            if (ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] <= 0)
            {
                int currentIndex = itemInventoryWindow._selectedIndex;
                for (; currentIndex + 1 < ItemManager.Instance.itemInventory.emptySpaceStartIndex; currentIndex++)
                {
                    ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex + 1];
                    ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex + 1];
                    for (int i = 0; i < ItemManager.Instance.itemInventory._equipToolIndex.Length; i++)
                    {
                        if (currentIndex + 1 == ItemManager.Instance.itemInventory._equipToolIndex[i])
                        {
                            ItemManager.Instance.itemInventory._equipToolIndex[i] -= 1;
                            break;
                        }
                    }
                }
                ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemType.Null;
                ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = 0;
                ItemManager.Instance.itemInventory.emptySpaceStartIndex -= 1;
                itemInventoryWindow.SetExplan(ItemInventoryWindow.notSelect);
                initialize();
            }
        }
        else if (ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]].Tag == ItemTag.Tool)
        {
            ToolItemTag selectedItemToolType = ((ToolItemData)ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]]).ToolType;
            for (int i = 0; i < ItemManager.Instance.itemInventory._equipToolIndex.Length; i++)
            {
                if (selectedItemToolType == (ToolItemTag)i)
                {
                    if (ItemManager.Instance.itemInventory._equipToolIndex[i] != itemInventoryWindow._selectedIndex)
                    {
                        ItemManager.Instance.itemInventory._equipToolIndex[i] = itemInventoryWindow._selectedIndex;
                        onChangeTool?.Invoke(selectedItemToolType, ((ToolItemData)ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]]).Level);
                    }
                    else
                    {
                        ItemManager.Instance.itemInventory._equipToolIndex[i] = ItemInventory.notEquip;
                        onChangeTool?.Invoke(selectedItemToolType, ItemInventory.notEquip);
                    }
                    break;
                }
            }
        }
        else if (ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]].Tag == ItemTag.Deployment)
        {
            ItemManager.Instance.OnHousingMode();
            if (itemInventoryWindow.gameObject.activeSelf == true)
            {
                itemInventoryWindow._saveSelectedIndex = itemInventoryWindow._selectedIndex;
                itemInventoryWindow.PreOnDisble();
                itemInventoryWindow.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
        itemInventoryWindow.RefreshItemInventory();
        CraftingsWindow.CheckCanMakeTool();
    }

    public void ItemDump()
    {
        if (ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]].Tag != ItemTag.Deployment && ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]].Tag != ItemTag.Etc) {
            ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] -= 1;
            if (ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] <= 0)
            {
                for (int i = 0; i < ItemManager.Instance.itemInventory._equipToolIndex.Length; i++)
                {
                    if (ItemManager.Instance.itemInventory._equipToolIndex[i] == itemInventoryWindow._selectedIndex)
                    {
                        ItemManager.Instance.itemInventory._equipToolIndex[i] = ItemInventory.notEquip;
                        onChangeTool?.Invoke((ToolItemTag)i, ItemInventory.notEquip);
                        break;
                    }
                }
                int currentIndex = itemInventoryWindow._selectedIndex;
                for (; currentIndex + 1 < ItemManager.Instance.itemInventory.emptySpaceStartIndex; currentIndex++)
                {
                    ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex + 1];
                    ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex + 1];
                    for (int i = 0; i < ItemManager.Instance.itemInventory._equipToolIndex.Length; i++)
                    {
                        if (currentIndex + 1 == ItemManager.Instance.itemInventory._equipToolIndex[i])
                        {
                            ItemManager.Instance.itemInventory._equipToolIndex[i] -= 1;
                            break;
                        }
                    }
                }
                ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemType.Null;
                ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = 0;
                ItemManager.Instance.itemInventory.emptySpaceStartIndex -= 1;
                itemInventoryWindow.SetExplan(ItemInventoryWindow.notSelect);
                initialize();
            }
            itemInventoryWindow.RefreshItemInventory();
            CraftingsWindow.CheckCanMakeTool();
        }
    }

    public void AfterItemUse() 
    {
        ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] -= 1;
        if (ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] <= 0)
        {
            int currentIndex = itemInventoryWindow._selectedIndex;
            for (; currentIndex + 1 < ItemManager.Instance.itemInventory.emptySpaceStartIndex; currentIndex++)
            {
                ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex + 1];
                ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex + 1];
                for (int i = 0; i < ItemManager.Instance.itemInventory._equipToolIndex.Length; i++)
                {
                    if (currentIndex + 1 == ItemManager.Instance.itemInventory._equipToolIndex[i])
                    {
                        ItemManager.Instance.itemInventory._equipToolIndex[i] -= 1;
                        break;
                    }
                }
            }
            ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemType.Null;
            ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = 0;
            ItemManager.Instance.itemInventory.emptySpaceStartIndex -= 1;
            ItemManager.Instance.SetUpItemPosition = ItemManager.Instance.SetUpAItem.transform.position;
            itemInventoryWindow.SetExplan(ItemInventoryWindow.notSelect);
            initialize();
        }
        itemInventoryWindow.RefreshItemInventory();
        CraftingsWindow.CheckCanMakeTool();
    }
    // Update is called once per frame

}
