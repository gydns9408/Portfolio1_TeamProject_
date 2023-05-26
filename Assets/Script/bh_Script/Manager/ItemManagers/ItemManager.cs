using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ItemType
{
    Strawberry = 0,
    Avocado,
    Peanut,
    Firewood,
    FirewoodX3,
    FirewoodX5,
    Gazami,
    Galchi,
    Shark,
    Stone,
    Iron,
    Gold,
    StrawberryGazamiZorim,
    AvocadoGrilledGalchi,
    PeanutSteamedShark,
    EscapeShip,
    StoneAxe,
    IronAxe,
    GoldAxe,
    StonePickaxe,
    IronPickaxe,
    GoldPickaxe,
    StoneSickle,
    IronSickle,
    GoldSickle,
    StoneFishingrod,
    IronFishingrod,
    GoldFishingrod,
    CraftingTable,
    Null
}

public class ItemManager : Singleton<ItemManager>
{
    ItemInventory itemsInventory;
    public ItemInventory itemInventory
    {
        get => itemsInventory;
        set => itemsInventory = value;
    }

    [SerializeField]
    int ItemInventoryMaxSpace;
    public int itemInventoryMaxSpace
    {
        get => ItemInventoryMaxSpace;
    }

    [SerializeField]
    int ItemInventoryWindowMaxAmount;
    public int itemInventoryWindowMaxAmount
    {
        get => ItemInventoryWindowMaxAmount;
    }

    [SerializeField]
    int NotDropItemTypeAmount = 1;
    public int notDropItemTypeAmount
    {
        get => NotDropItemTypeAmount;
    }

    // Start is called before the first frame update
    [SerializeField]
    List<ItemData> itemDatas;
    public ItemData this[ItemType type]
    {
        get { return itemDatas[(int)type]; }
        set { itemDatas[(int)type] = value; }
    }

    int ItemTypeCount;
    DropItemPool[] dropItemPools;
    const int notSelect = -1;

    HousingAction housingAction;
    bool isHousingMode = false;
    public bool IsHousingMode => isHousingMode;

    SetUpItem setUpItem;
    public SetUpItem SetUpAItem => setUpItem;
    CraftingWindow craftingsWindow;
    public CraftingWindow CraftingsWindow => craftingsWindow;

    [SerializeField]
    Vector3 setUpItemPosition;
    public Vector3 SetUpItemPosition
    {
        get => setUpItemPosition;
        set => setUpItemPosition = value;
    }

    public Action OnHousingmode;
    public Action OffHousingmode;

    WaitForSeconds wait = new WaitForSeconds(0.5f);

    protected override void PreInitialize()
    {
        if (initialized == false)
        {
            base.PreInitialize();
            ItemTypeCount = System.Enum.GetValues(typeof(ItemType)).Length - notDropItemTypeAmount - 1;
            dropItemPools = new DropItemPool[ItemTypeCount];
            for (int i = 0; i < ItemTypeCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                dropItemPools[i] = childTransform.GetComponent<DropItemPool>();
            }
            itemInventory = GetComponentInChildren<ItemInventory>();
            itemInventory.ItemAmountArray = new int[itemInventoryMaxSpace];
            itemInventory.ItemTypeArray = new ItemType[itemInventoryMaxSpace];
            itemInventory._equipToolIndex = new int[System.Enum.GetValues(typeof(ToolItemTag)).Length];
            for (int i = 0; i < itemInventoryMaxSpace; i++)
            {
                itemInventory.ItemTypeArray[i] = ItemType.Null;
            }
            for (int i = 0; i < itemInventory._equipToolIndex.Length; i++) 
            {
                itemInventory._equipToolIndex[i] = notSelect;
            }
            housingAction = new HousingAction();
        }
    }

    protected override void Initialize()
    {
        for (int i = 0; i < ItemTypeCount; i++)
        {
            dropItemPools[i]?.MakeObjectPool();
        }
        itemInventory.ItemsInventoryWindow = FindObjectOfType<ItemInventoryWindow>();
        if (itemInventory.ItemsInventoryWindow != null) {
            itemInventory.ItemsInventoryWindow.ExplanRoom = FindObjectOfType<ItemInventoryWindowExplanRoom>();
            itemInventory.ItemsInventoryWindow.ToolItemTag_Length = System.Enum.GetValues(typeof(ToolItemTag)).Length;
            PlayerBase playerbase = FindObjectOfType<PlayerBase>();
            CraftingWindow craftingWindow = FindObjectOfType<CraftingWindow>();
            playerbase.onInventory += itemInventory.ItemsInventoryWindow.OnAndOff;
            itemInventory.ItemsInventoryWindow.RefreshItemInventory();
            itemInventory.ItemsInventoryWindow.ExplanRoom.ItemInventoryWindow_p = itemInventory.ItemsInventoryWindow;
            itemInventory.ItemsInventoryWindow.ExplanRoom._itemUseButton.onClick.AddListener(itemInventory.ItemsInventoryWindow.ExplanRoom.ItemUse);
            itemInventory.ItemsInventoryWindow.ExplanRoom._itemDumpButton.onClick.AddListener(itemInventory.ItemsInventoryWindow.ExplanRoom.ItemDump);
            craftingsWindow = craftingWindow;
            itemInventory.ItemsInventoryWindow.ExplanRoom.CraftingsWindow = craftingWindow;
            itemInventory.ItemsInventoryWindow.ExplanRoom.gameObject.SetActive(false);
            itemInventory.ItemsInventoryWindow.gameObject.SetActive(false);
            
            playerbase.onMaking += craftingWindow.OpenCraftingWindow;
            craftingWindow.StartCraft.onClick.AddListener(craftingWindow.OnMakeTool);
            craftingWindow.MenuCloseButton.onClick.AddListener(craftingWindow.CloseMenu);
            craftingWindow.gameObject.SetActive(false);
        }
        setUpItem = FindObjectOfType<SetUpItem>();
        OnHousingmode = null;
        OffHousingmode = null;
        StopAllCoroutines();
        StartCoroutine(RefreshEmptySpaceStartIndex());
    }

    private void OnDisable()
    {
        OffHousingMode(false);
    }

    public GameObject GetObject(ItemType itemType)
    {
        GameObject result = dropItemPools[(int)itemType]?.GetObject().gameObject;
        return result;
    }
    //test
    void SetUpObject(InputAction.CallbackContext _)
    {
        //Vector2 screenPosition = Mouse.current.position.ReadValue();
        //Vector2 aaa = UnityEngine.Camera.main.ScreenToWorldPoint(screenPosition);
        //Vector3 newPositon = new Vector3(aaa.x, 0, aaa.y);
        //Ray ray = UnityEngine.Camera.main.ScreenPointToRay(newPositon);
        bool isUse = false;
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenPosition);
        int layerMask = (1 << LayerMask.NameToLayer("Player"));
        layerMask = ~layerMask;
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject.CompareTag("HousingPlace"))
            {
                setUpItem.gameObject.SetActive(true);
                setUpItem.SetUp();
                setUpItem.transform.position = hit.point;
                isUse = true;
            }
            Debug.Log(hit.collider.gameObject.name);
        }
        OffHousingMode(isUse);
    }

    public void OnHousingMode() 
    {
        if (isHousingMode == false) 
        { 
            isHousingMode = true;
            housingAction.Player.Enable();
            housingAction.Player.SetUp.performed += SetUpObject;
            OnHousingmode?.Invoke();
            //Debug.Log("Onせせせせ");

        }   
    }

    void OffHousingMode(bool isUse) 
    {
        if (isHousingMode == true)
        {
            isHousingMode = false;
            housingAction.Player.SetUp.performed -= SetUpObject;
            housingAction.Player.Disable();
            //Debug.Log("OFFせせせせ");
            if (itemInventory.ItemsInventoryWindow.gameObject.activeSelf == false)
            {
                itemInventory.ItemsInventoryWindow.gameObject.SetActive(true);
                itemInventory.ItemsInventoryWindow.AfterItemUse(isUse);
            }
            OffHousingmode?.Invoke();
        }
    }

    public void WithdrawObject()
    {
        setUpItem.gameObject.SetActive(false);
    }

    IEnumerator RefreshEmptySpaceStartIndex()
    {
        yield return wait;
        for (int i = 0; i < itemInventoryMaxSpace; i++)
        {
            if (itemInventory.ItemTypeArray[i] == ItemType.Null)
            {
                itemInventory.emptySpaceStartIndex = i;
                break;
            }
        }
        if (setUpItem != null)
        {
            if (itemInventory.FindItem(ItemType.CraftingTable, 1))
            {
                setUpItem.gameObject.SetActive(false);
            }
            else
            {
                setUpItem.transform.position = setUpItemPosition;
            }
        }

    }
}
