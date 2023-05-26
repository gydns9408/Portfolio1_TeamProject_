using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemToolTips : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI _itemName;
    [SerializeField]
    public TextMeshProUGUI _itemExplan;
    [SerializeField]
    public TextMeshProUGUI _itemTag;
    public Button _itemUseButton;
    public Button _itemDumpButton;

    ItemInventoryWindow itemInventoryWindow;

    //void Awake()
    //{
    //    Transform child1 = transform.GetChild(0);
    //    _itemName = child1.GetComponent<TextMeshProUGUI>();
    //    Transform child2 = transform.GetChild(1);
    //    _itemExplan = child2.GetComponent<TextMeshProUGUI>();
    //    Transform child3 = transform.GetChild(2);
    //    _itemTag = child3.GetComponent<TextMeshProUGUI>();
    //    Transform child4 = transform.GetChild(3);
    //    _itemUseButton = child4.GetComponent<Button>();
    //    Transform child5 = transform.GetChild(4);
    //    _itemDumpButton = child5.GetComponent<Button>();

    //    initialize();
    //}

    //void Start()
    //{
    //    gameObject.SetActive(false);
    //    itemInventoryWindow = FindObjectOfType<ItemInventoryWindow>();
    //    _itemUseButton.onClick.AddListener(ItemUse);
    //    _itemDumpButton.onClick.AddListener(ItemDump);
    //}

    //void OnDisable()
    //{
    //    initialize();
    //}

    //void initialize()
    //{
    //    _itemName.text = string.Empty;
    //    _itemExplan.text = string.Empty;
    //    _itemTag.text = string.Empty;
    //    _itemUseButton.gameObject.SetActive(false);
    //    _itemDumpButton.gameObject.SetActive(false);
    //}

    //void ItemUse()
    //{
    //    if (ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]].Tag == ItemTag.Food)
    //    {
    //        ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] -= 1;
    //        if (ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] <= 0)
    //        {
    //            int currentIndex = itemInventoryWindow._selectedIndex;
    //            for (; currentIndex + 1 < ItemManager.Instance.itemInventory.emptySpaceStartIndex; currentIndex++)
    //            {
    //                ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex + 1];
    //                ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex + 1];
    //                if (currentIndex + 1 == ItemManager.Instance.itemInventory._equipToolIndex)
    //                {
    //                    ItemManager.Instance.itemInventory._equipToolIndex -= 1;
    //                }
    //            }
    //            ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemType.Null;
    //            ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = 0;
    //            ItemManager.Instance.itemInventory.emptySpaceStartIndex -= 1;
    //            itemInventoryWindow.SetExplan(ItemInventoryWindow.notSelect);
    //            initialize();
    //        }
    //    }
    //    else if (ItemManager.Instance[ItemManager.Instance.itemInventory.ItemTypeArray[itemInventoryWindow._selectedIndex]].Tag == ItemTag.Tool)
    //    {
    //        if (ItemManager.Instance.itemInventory._equipToolIndex != itemInventoryWindow._selectedIndex)
    //        {
    //            ItemManager.Instance.itemInventory._equipToolIndex = itemInventoryWindow._selectedIndex;
    //        }
    //        else
    //        {
    //            ItemManager.Instance.itemInventory._equipToolIndex = ItemInventory.notEquip;
    //        }
    //    }
    //    itemInventoryWindow.RefreshItemInventory();
    //}

    //void ItemDump()
    //{
    //    ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] -= 1;
    //    if (ItemManager.Instance.itemInventory.ItemAmountArray[itemInventoryWindow._selectedIndex] <= 0)
    //    {
    //        if (ItemManager.Instance.itemInventory._equipToolIndex == itemInventoryWindow._selectedIndex)
    //        {
    //            ItemManager.Instance.itemInventory._equipToolIndex = ItemInventory.notEquip;
    //        }
    //        int currentIndex = itemInventoryWindow._selectedIndex;
    //        for (; currentIndex + 1 < ItemManager.Instance.itemInventory.emptySpaceStartIndex; currentIndex++)
    //        {
    //            ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex + 1];
    //            ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex + 1];
    //            if (currentIndex + 1 == ItemManager.Instance.itemInventory._equipToolIndex)
    //            {
    //                ItemManager.Instance.itemInventory._equipToolIndex -= 1;
    //            }
    //        }
    //        ItemManager.Instance.itemInventory.ItemTypeArray[currentIndex] = ItemType.Null;
    //        ItemManager.Instance.itemInventory.ItemAmountArray[currentIndex] = 0;
    //        ItemManager.Instance.itemInventory.emptySpaceStartIndex -= 1;
    //        itemInventoryWindow.SetExplan(ItemInventoryWindow.notSelect);
    //        initialize();
    //    }
    //    itemInventoryWindow.RefreshItemInventory();
    //}
}
