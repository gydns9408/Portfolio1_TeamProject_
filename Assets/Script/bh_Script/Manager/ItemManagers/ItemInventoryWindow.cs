using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventoryWindow : MonoBehaviour
{
    // Start is called before the first frame update

    public const int notSelect = -1;
    int _SelectedIndex = notSelect;
    public int _selectedIndex
    {
        get => _SelectedIndex;
        set => _SelectedIndex = value;
    }

    int _SaveSelectedIndex = notSelect;
    public int _saveSelectedIndex
    {
        set => _SaveSelectedIndex = value;
    }

    int toolItemTag_Length;
    public int ToolItemTag_Length
    {
        get => toolItemTag_Length;
        set => toolItemTag_Length = value;
    }

    Color InventoryNormalColor = Color.white;
    public Color inventoryNormalColor
    {
        get => InventoryNormalColor;
    }

    ItemInventoryWindowRoom[] itemInventoryWindowRooms;
    ItemInventoryWindowExplanRoom explanRoom;
    public ItemInventoryWindowExplanRoom ExplanRoom
    {
        get => explanRoom;
        set => explanRoom = value;
    }

    string[] itemTagString;

    void Awake()
    {
        itemInventoryWindowRooms = GetComponentsInChildren<ItemInventoryWindowRoom>();
        itemTagString = new string[] { "음식 아이템", "재료 아이템", "장비 아이템", "기타 아이템", "배치 아이템" };
    }

    private void Start()
    {
        //explanRoom = FindObjectOfType<ItemInventoryWindowExplanRoom>();
        //toolItemTag_Length = System.Enum.GetValues(typeof(ToolItemTag)).Length;
        //PlayerBase playerbase = FindObjectOfType<PlayerBase>();
        //playerbase.onInventory += OnAndOff;
        //RefreshItemInventory();
    }

    private void OnEnable()
    {
        if (_SaveSelectedIndex != notSelect) 
        {
            _selectedIndex = _SaveSelectedIndex;
            _saveSelectedIndex = notSelect;
        }
        if (explanRoom != null) 
        {
            RefreshItemInventory();
        }
    }

    public void PreOnDisble()
    {
        if (_selectedIndex != notSelect)
        {
            itemInventoryWindowRooms[_selectedIndex]._panelImage.color = inventoryNormalColor;
            _selectedIndex = notSelect;
        }
    }

    //private void OnDestroy()
    //{
    //    PlayerBase playerbase = FindObjectOfType<PlayerBase>();
    //    playerbase.onInventory -= OnAndOff;
    //}

    public void RefreshItemInventory()
    {
        for (int i = 0; i < ItemManager.Instance.itemInventoryMaxSpace; i++)
        {
            if (ItemManager.Instance.itemInventory.ItemTypeArray[i] != ItemType.Null)
            {
                if (ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[i]].Tag != ItemTag.Tool)
                {
                    itemInventoryWindowRooms[i].SetSpace(ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[i]].IconSprite, ItemManager.Instance.itemInventory.ItemAmountArray[i]);
                }
                else
                {
                    bool nowEquip = false;
                    for (int j = 0; j < ToolItemTag_Length; j++) 
                    {
                        if (i == ItemManager.Instance.itemInventory._equipToolIndex[j])
                        {
                            nowEquip = true;
                            break;
                        }
                    }

                    itemInventoryWindowRooms[i].SetSpace(ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[i]].IconSprite, nowEquip);
                }
            }
            else
            {
                itemInventoryWindowRooms[i].DisableComponent();
            }
        }

    }

    public void SetExplan(int index)
    {
        if (index == notSelect)
        {
            itemInventoryWindowRooms[_selectedIndex]._panelImage.color = inventoryNormalColor;
            _selectedIndex = index;
            return;
        }
        if (_selectedIndex != notSelect)
        {
            itemInventoryWindowRooms[_selectedIndex]._panelImage.color = inventoryNormalColor;
        }
        itemInventoryWindowRooms[index]._panelImage.color = Color.green;
        explanRoom._itemName.text = ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[index]].ItemName;
        explanRoom._itemExplan.text = ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[index]].Explan;
        explanRoom._itemTag.text = itemTagString[(int)(ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[index]].Tag)];
        explanRoom._itemUseButton.gameObject.SetActive(true);
        explanRoom._itemDumpButton.gameObject.SetActive(true);
        _selectedIndex = index;
    }
    // Update is called once per frame

    public void AfterItemUse(bool isUse)
    {
        if (ExplanRoom.gameObject.activeSelf == false)
        {
            ExplanRoom.gameObject.SetActive(true);
            SetExplan(_selectedIndex);
        }
        if (isUse)
        {
            ExplanRoom.AfterItemUse();
        }
    }

    public void OnAndOff()
    {
        if (!ItemManager.Instance.IsHousingMode) {
            if (gameObject.activeSelf == true)
            {
                PreOnDisble();
                gameObject.SetActive(false);
                explanRoom.gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                explanRoom.gameObject.SetActive(true);
            }
        }
    }
}
