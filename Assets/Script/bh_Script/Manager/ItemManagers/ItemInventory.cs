using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    int[] itemAmountArray;
    public int[] ItemAmountArray
    {
        get => itemAmountArray;
        set { itemAmountArray = value; }
    }

    ItemType[] itemTypeArray;
    public ItemType[] ItemTypeArray
    {
        get => itemTypeArray;
        set { itemTypeArray = value; }
    }

    //public const int emptySpace = -1;
    int EmptySpaceStartIndex = 0;
    public int emptySpaceStartIndex
    {
        get => EmptySpaceStartIndex;
        set => EmptySpaceStartIndex = value;
    }

    public const int notEquip = -1;
    int[] _EquipToolIndex;
    public int[] _equipToolIndex
    {
        get => _EquipToolIndex;
        set => _EquipToolIndex = value;
    }

    ItemInventoryWindow itemsInventoryWindow;
    public ItemInventoryWindow ItemsInventoryWindow
    {
        get => itemsInventoryWindow;
        set => itemsInventoryWindow = value; 
    }

    public void AddItem(ItemType itemType, int amount) 
    {
        if (ItemManager.Instance[itemType].Tag != ItemTag.Tool)
        {
            bool inventoryAlreadyhave = false; // 아이템 인벤토리에 특정 아이템이 있는지 여부를 확인하는 bool 변수
            for (int i = 0; i < emptySpaceStartIndex; i++)
            {
                if (ItemTypeArray[i] == itemType && ItemAmountArray[i] < ItemManager.Instance.itemInventoryWindowMaxAmount)
                {
                    if (ItemAmountArray[i] + amount <= ItemManager.Instance.itemInventoryWindowMaxAmount)
                    {
                        ItemAmountArray[i] += amount;
                        inventoryAlreadyhave = true;
                    }
                    else 
                    {
                        int amountOfPart = ItemManager.Instance.itemInventoryWindowMaxAmount - ItemAmountArray[i];
                        ItemAmountArray[i] += amountOfPart;
                        AddItem(itemType, amount - amountOfPart);
                        inventoryAlreadyhave = true;
                    }
                    break;
                } //아이템 인벤토리에서 Strawberry가 있는지 검사하고 있다면, 그 위치에 개수 1개 추가
            }
            if (!inventoryAlreadyhave) // 만약 아이템 인벤토리에 Strawberry가 없다면
            {
                if (emptySpaceStartIndex < ItemManager.Instance.itemInventoryMaxSpace) // 그리고 만약 아이템 인벤토리가 꽉 차지 않았다면
                {
                    if (amount <= ItemManager.Instance.itemInventoryWindowMaxAmount)
                    {
                        ItemTypeArray[emptySpaceStartIndex] = itemType;
                        ItemAmountArray[emptySpaceStartIndex] += amount;
                        emptySpaceStartIndex++;
                    }
                    else 
                    {
                        ItemTypeArray[emptySpaceStartIndex] = itemType;
                        ItemAmountArray[emptySpaceStartIndex] += ItemManager.Instance.itemInventoryWindowMaxAmount;
                        emptySpaceStartIndex++;
                        AddItem(itemType, amount - ItemManager.Instance.itemInventoryWindowMaxAmount);
                    }
                }
            }
        }
        else
        {
            if (emptySpaceStartIndex < ItemManager.Instance.itemInventoryMaxSpace)
            {
                if (amount <= 1)
                {
                    ItemTypeArray[emptySpaceStartIndex] = itemType;
                    ItemAmountArray[emptySpaceStartIndex] = 1;
                    emptySpaceStartIndex++;
                }
                else 
                {
                    ItemTypeArray[emptySpaceStartIndex] = itemType;
                    ItemAmountArray[emptySpaceStartIndex] = 1;
                    emptySpaceStartIndex++;
                    AddItem(itemType, amount - 1);
                }
            }
        }
        if (itemsInventoryWindow != null) {
            itemsInventoryWindow.RefreshItemInventory();
        }
    }

    public void SubtractItem(ItemType itemType, int amount) 
    {
        int i = 0;
        if (ItemManager.Instance[itemType].Tag != ItemTag.Tool)
        {
            for (; i < emptySpaceStartIndex; i++)
            {
                if (ItemTypeArray[i] == itemType)
                {
                    if (ItemAmountArray[i] >= amount)
                    {
                        ItemAmountArray[i] -= amount;
                        amount = 0;
                    }
                    else
                    {
                        amount -= ItemAmountArray[i];
                        ItemAmountArray[i] = 0;
                    }
                    break;
                }
            }
        }
        else 
        {
            for (; i < emptySpaceStartIndex; i++)
            {
                if (ItemTypeArray[i] == itemType)
                {
                    ItemAmountArray[i] = 0;
                    amount -= 1;
                    break;
                }
            }
        }
        if (i == emptySpaceStartIndex) 
        {
            return;
        }
        if (ItemAmountArray[i] <= 0)
        {
            for (int j = 0; j < _equipToolIndex.Length; j++)
            {
                if (_equipToolIndex[j] == i)
                {
                    _equipToolIndex[j] = notEquip;
                    itemsInventoryWindow.ExplanRoom.onChangeTool?.Invoke((ToolItemTag)j, ItemInventory.notEquip);
                    break;
                }
            }
            if (itemsInventoryWindow != null)
            {
                if (itemsInventoryWindow._selectedIndex == i)
                {
                    itemsInventoryWindow.SetExplan(ItemInventoryWindow.notSelect);
                    itemsInventoryWindow.ExplanRoom.initialize();
                }
            }
            int currentIndex = i;
            for (; currentIndex + 1 < emptySpaceStartIndex; currentIndex++)
            {
                ItemTypeArray[currentIndex] = ItemTypeArray[currentIndex + 1];
                ItemAmountArray[currentIndex] = ItemAmountArray[currentIndex + 1];
                for (int j = 0; j < _equipToolIndex.Length; j++)
                {
                    if (currentIndex + 1 == _equipToolIndex[j])
                    {
                        _equipToolIndex[j] -= 1;
                        break;
                    }
                }
                if (currentIndex + 1 == itemsInventoryWindow._selectedIndex) 
                {
                    itemsInventoryWindow.SetExplan(itemsInventoryWindow._selectedIndex - 1);
                }
            }
            ItemTypeArray[currentIndex] = ItemType.Null;
            ItemAmountArray[currentIndex] = 0;
            emptySpaceStartIndex -= 1;
        }
        if (amount > 0) 
        {
            SubtractItem(itemType, amount);
        }
        if (itemsInventoryWindow != null)
        {
            itemsInventoryWindow.RefreshItemInventory();
        }
    }

    public int GetEquipToolLevel(ToolItemTag toolItemTag) 
    {
        if (_equipToolIndex[(int)toolItemTag] != notEquip) {
            return ((ToolItemData)ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[_equipToolIndex[(int)toolItemTag]]]).Level;
        } 
        else 
        {
            return notEquip;
        }
    }

    public bool FindItem(ItemType itemType, int amount) 
    {
        bool isInventoryHave = false; // 아이템 인벤토리에 특정 아이템이 있는지 여부를 확인하는 bool 변수
        for (int i = 0; i < emptySpaceStartIndex; i++)
        {
            if (ItemTypeArray[i] == itemType)
            {
                if (ItemAmountArray[i] >= amount)
                {
                    isInventoryHave = true;
                    break;
                }
                else 
                {
                    amount -= ItemAmountArray[i];
                }
            } 
        }
        return isInventoryHave;
    }

    public void MakeItem(ItemType itemType, int amount)
    {
        for (int i = 0; i < ItemManager.Instance[itemType].ProductionMaterialTypeList.Count; i++)
        {
            if (!FindItem(ItemManager.Instance[itemType].ProductionMaterialTypeList[i], ItemManager.Instance[itemType].ProductionMaterialAmountList[i] * amount))
            {
                return;
            }
        }

        for (int i = 0; i < ItemManager.Instance[itemType].ProductionMaterialTypeList.Count; i++)
        {
            SubtractItem(ItemManager.Instance[itemType].ProductionMaterialTypeList[i], ItemManager.Instance[itemType].ProductionMaterialAmountList[i] * amount);
        }
        AddItem(itemType, amount);
    }

    public void LoadInventoryData(int[] typeArray, int[] amoutArray, int[] equipToolArray)
    {
        for (int i = 0; i < ItemManager.Instance.itemInventoryMaxSpace; i++) 
        {
            ItemTypeArray[i] = (ItemType)typeArray[i];
        }
        ItemAmountArray = amoutArray;
        _equipToolIndex = equipToolArray;
    }
}
