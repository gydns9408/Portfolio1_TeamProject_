using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingWindow : MonoBehaviour
{
    PlayerBase player;
    Button startCraft;
    public Button StartCraft => startCraft;
    Button menuCloseButton; // 닫기 버튼 추가
    public Button MenuCloseButton => menuCloseButton;

    public const int notSelect = -1;
    int _SelectedIndex = notSelect;
    public int _selectedIndex
    {
        get => _SelectedIndex;
        set => _SelectedIndex = value;
    }

    CraftingWindowRoom[] craftingWindowRooms;

    TextMeshProUGUI _selectItemName;
    TextMeshProUGUI _selectItemExplan;
    TextMeshProUGUI _selectItemTag;

    TextMeshProUGUI _itemMakeGuideText;

    Image[] productionMaterialImages;
    TextMeshProUGUI[] productionMaterialTexts;

    [SerializeField]
    ItemType[] makePossibleItems;

    string[] itemTagString;

    public bool canMakeTool = false;

    public Action DontAction;
    public Action DoAction;

    public Action makeShip;


    private void Awake()
    {
        startCraft = GameObject.Find("startCraft").GetComponent<Button>();

        Transform productionMaterialTransform = transform.GetChild(0);
        _itemMakeGuideText = productionMaterialTransform.GetChild(productionMaterialTransform.childCount - 1).GetComponent<TextMeshProUGUI>();
        productionMaterialImages = new Image[productionMaterialTransform.childCount - 3];
        productionMaterialTexts = new TextMeshProUGUI[productionMaterialTransform.childCount - 3];
        for (int i = 0; i < productionMaterialImages.Length; i++) 
        {
            productionMaterialImages[i] = productionMaterialTransform.GetChild(i).GetChild(0).GetComponent<Image>();
            productionMaterialTexts[i] = productionMaterialTransform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        menuCloseButton = transform.GetChild(2).GetComponent<Button>();     // 버튼 찾기
        craftingWindowRooms = GetComponentsInChildren<CraftingWindowRoom>();

        Transform explanTransform = transform.GetChild(3);
        _selectItemName = explanTransform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _selectItemExplan = explanTransform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _selectItemTag = explanTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
        itemTagString = new string[] { "음식 아이템", "재료 아이템", "장비 아이템", "기타 아이템", "배치 아이템" };

        Transform buttonTransform = transform.GetChild(4);
        Button rearrangeCraftingTableButton = buttonTransform.GetComponent<Button>();
        rearrangeCraftingTableButton.onClick.AddListener(RearrangeCraftingTable);
    }
    private void Start()
    {
        //player = FindObjectOfType<PlayerBase>();
        //gameObject.SetActive(false);
        //player.onMaking += OpenCraftingWindow;
        //startCraft.onClick.AddListener(OnMakeTool);
        //menuCloseButton.onClick.AddListener(CloseMenu);
    }

    public void OnMakeTool()
    {
        Debug.Log($"canMakeTool : {canMakeTool}");
        if(canMakeTool == true)
        {
            //Debug.Log("제작 가능");
           for (int i = 0; i < ItemManager.Instance[makePossibleItems[_selectedIndex]].ProductionMaterialTypeList.Count; i++)
            {
                ItemManager.Instance.itemInventory.SubtractItem(ItemManager.Instance[makePossibleItems[_selectedIndex]].ProductionMaterialTypeList[i], ItemManager.Instance[makePossibleItems[_selectedIndex]].ProductionMaterialAmountList[i] * 1);
            }
            ItemManager.Instance.itemInventory.AddItem(makePossibleItems[_selectedIndex], 1);
            _itemMakeGuideText.text = $"{ItemManager.Instance[makePossibleItems[_selectedIndex]].ItemName} 1개를 제작완료하였습니다!";
            CheckCanMakeTool();
            if (_selectedIndex == 12)
            {
                makeShip?.Invoke();
            }
        }
    }

    public void OpenCraftingWindow()
    {
       if (gameObject.activeSelf == true)
       {
            CleanMenu();
            gameObject.SetActive(false);
            DoAction.Invoke();
       }
       else
       {
           gameObject.SetActive(true);
           RefreshCraftingWindow();
           DontAction?.Invoke();
           /*startCraft.interactable = false;*/  //불값의 변경에 따른 클릭 가능 여부 f키 누를때마다 체크
           canMakeTool = false;
        }
    }

    /// <summary>
    /// 버튼 닫기용 함수
    /// </summary>
    public void CloseMenu()
    {
        CleanMenu();
        gameObject.SetActive(false);
        DoAction?.Invoke();
    }

    private void RearrangeCraftingTable()
    {
        if (ItemManager.Instance.itemInventory.emptySpaceStartIndex < ItemManager.Instance.itemInventoryMaxSpace)
        {
            CleanMenu();
            ItemManager.Instance.itemInventory.AddItem(ItemType.CraftingTable, 1);
            ItemManager.Instance.WithdrawObject();
            gameObject.SetActive(false);
            DoAction?.Invoke();
        }
        else 
        {
            _itemMakeGuideText.text = "아이템 칸이 모자랍니다. 아이템 칸을 1칸 이상 비워주세요.";
            startCraft.interactable = false;
        }
    }

    void CleanMenu()
    {
        for (int i = 0; i < craftingWindowRooms.Length; i++)
        {
            craftingWindowRooms[i].DisableComponent();
        }
        _selectItemName.text = string.Empty;
        _selectItemExplan.text = string.Empty;
        _selectItemTag.text = string.Empty;
        for (int i = 0; i < productionMaterialImages.Length; i++) 
        {
            productionMaterialImages[i].enabled = false;
            productionMaterialTexts[i].text = string.Empty;
        }
    }

    public void RefreshCraftingWindow()
    {
        for (int i = 0; i < craftingWindowRooms.Length; i++)
        {
            if (i < makePossibleItems.Length)
            {
                craftingWindowRooms[i].SetSpace(ItemManager.Instance[makePossibleItems[i]].IconSprite);
            }
            else
            {
                craftingWindowRooms[i].DisableComponent();
            }
        }

        if (_selectedIndex != notSelect)
        {
            craftingWindowRooms[_selectedIndex]._panelImage.color = Color.white;
            _selectedIndex = notSelect;
        }


        for (int i = 0; i < productionMaterialImages.Length; i++)
        {
            productionMaterialImages[i].enabled = false;
            productionMaterialTexts[i].text = string.Empty;
        }
        
        _itemMakeGuideText.text = string.Empty;
    }

    public void SetExplan(int index)
    {
        if (index == notSelect)
        {
            craftingWindowRooms[_selectedIndex]._panelImage.color = Color.white;
            _selectedIndex = index;
            return;
        }
        if (_selectedIndex != notSelect)
        {
            craftingWindowRooms[_selectedIndex]._panelImage.color = Color.white;
        }
        craftingWindowRooms[index]._panelImage.color = Color.green;
        _selectItemName.text = ItemManager.Instance[makePossibleItems[index]].ItemName;
        _selectItemExplan.text = ItemManager.Instance[makePossibleItems[index]].Explan;
        _selectItemTag.text = itemTagString[(int)(ItemManager.Instance[makePossibleItems[index]].Tag)];
        for (int i = 0; i < productionMaterialImages.Length; i++)
        {
            if (i < ItemManager.Instance[makePossibleItems[index]].ProductionMaterialTypeList.Count)
            {
                productionMaterialImages[i].enabled = true;
                productionMaterialImages[i].sprite = ItemManager.Instance[ItemManager.Instance[makePossibleItems[index]].ProductionMaterialTypeList[i]].IconSprite;
                productionMaterialTexts[i].text = ItemManager.Instance[makePossibleItems[index]].ProductionMaterialAmountList[i].ToString();
            }
            else 
            {
                productionMaterialImages[i].enabled = false;
                productionMaterialTexts[i].text = string.Empty;
            }
        }
        
        _itemMakeGuideText.text = string.Empty;
        _selectedIndex = index;
        CheckCanMakeTool();
    }

    public void CheckCanMakeTool()
    {
        canMakeTool = true;
        if (_selectedIndex != notSelect)
        {
            if (ItemManager.Instance.itemInventory.emptySpaceStartIndex < ItemManager.Instance.itemInventoryMaxSpace)
            {
                for (int i = 0; i < ItemManager.Instance[makePossibleItems[_selectedIndex]].ProductionMaterialTypeList.Count; i++)
                {
                    if (!ItemManager.Instance.itemInventory.FindItem(ItemManager.Instance[makePossibleItems[_selectedIndex]].ProductionMaterialTypeList[i], ItemManager.Instance[makePossibleItems[_selectedIndex]].ProductionMaterialAmountList[i] * 1))
                    {
                        canMakeTool = false;
                        break;
                    }
                }
            }
            else
            {
                canMakeTool = false;
            }
        }
        else
        {
            canMakeTool = false;
        }
        startCraft.interactable = canMakeTool;
    }
}
