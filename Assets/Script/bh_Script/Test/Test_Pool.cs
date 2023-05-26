using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Test_Pool : Test_Base
{
    ItemInventoryWindow itemInventoryWindow;
    ItemInventoryWindowExplanRoom explanRoom;
    float time = 0f;
    //test

    void Start() 
    {
        //itemInventoryWindow = FindObjectOfType<ItemInventoryWindow>();
        //explanRoom = FindObjectOfType<ItemInventoryWindowExplanRoom>();
    }

    void Update()
    {
        time += Time.deltaTime;
    }

    protected override void DoAction1(InputAction.CallbackContext _)
    {
        //GameObject obj = ItemManager.Instance.GetObject(ItemType.Strawberry); // Tomato 게임오브젝트를 ItemManager에서 가져와 활성화
        //obj.transform.position = new Vector3(4.4f, 0.85f, -0.5f);
        //Debug.Log($"시작이야아아 : {time}");
        ItemManager.Instance.itemInventory.AddItem(ItemType.Stone, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Firewood, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Strawberry, 30);
        Debug.Log($"{ItemManager.Instance.SetUpItemPosition.x} {ItemManager.Instance.SetUpItemPosition.y} {ItemManager.Instance.SetUpItemPosition.z}");
        //ItemManager.Instance.itemInventory.MakeItem(ItemType.GoldPickaxe, 1);
    }

    protected override void DoAction2(InputAction.CallbackContext _)
    {
        //GameObject obj = ItemManager.Instance.GetObject(ItemType.Avocado); // Strawberry 게임오브젝트를 ItemManager에서 가져와 활성화
        //obj.transform.position = Vector3.up * 9;
        if (itemInventoryWindow == null) 
        {
            itemInventoryWindow = ItemManager.Instance.itemInventory.ItemsInventoryWindow;
            explanRoom = ItemManager.Instance.itemInventory.ItemsInventoryWindow.ExplanRoom;
        }


        if (itemInventoryWindow.gameObject.activeSelf == true)
        {
            itemInventoryWindow.PreOnDisble();
            itemInventoryWindow.gameObject.SetActive(false);
            explanRoom.gameObject.SetActive(false);
        }
        else 
        {
            itemInventoryWindow.gameObject.SetActive(true);
            explanRoom.gameObject.SetActive(true);
        }
    }

    protected override void DoAction3(InputAction.CallbackContext _)
    {
        //if (ItemManager.Instance.itemInventory.emptySpaceStartIndex < ItemManager.Instance.itemInventoryMaxSpace) 
        //{
        //    ItemManager.Instance.itemInventory.ItemTypeArray[ItemManager.Instance.itemInventory.emptySpaceStartIndex] = ItemType.StoneAxe;
        //    ItemManager.Instance.itemInventory.ItemAmountArray[ItemManager.Instance.itemInventory.emptySpaceStartIndex] += 1;
        //    ItemManager.Instance.itemInventory.emptySpaceStartIndex++;
        //}

        //ItemManager.Instance.itemInventory.AddItem(ItemType.CraftingTable, 1);

    }

    protected override void DoAction4(InputAction.CallbackContext _)
    {

        //bool inventoryAlreadyhave = false; // 아이템 인벤토리에 특정 아이템이 있는지 여부를 확인하는 bool 변수
        //for (int i = 0; i < ItemManager.Instance.itemInventory.emptySpaceStartIndex; i++) { 
        //    if (ItemManager.Instance.itemInventory.ItemTypeArray[i] == ItemType.Strawberry) 
        //    {
        //        ItemManager.Instance.itemInventory.ItemAmountArray[i] += 1;
        //        inventoryAlreadyhave = true;
        //        break;
        //    } //아이템 인벤토리에서 Strawberry가 있는지 검사하고 있다면, 그 위치에 개수 1개 추가
        //}
        //if (!inventoryAlreadyhave) // 만약 아이템 인벤토리에 Strawberry가 없다면
        //{
        //    if (ItemManager.Instance.itemInventory.emptySpaceStartIndex < ItemManager.Instance.itemInventoryMaxSpace) // 그리고 만약 아이템 인벤토리가 꽉 차지 않았다면
        //    {
        //        ItemManager.Instance.itemInventory.ItemTypeArray[ItemManager.Instance.itemInventory.emptySpaceStartIndex] = ItemType.Strawberry; 
        //        ItemManager.Instance.itemInventory.ItemAmountArray[ItemManager.Instance.itemInventory.emptySpaceStartIndex] += 1;
        //        ItemManager.Instance.itemInventory.emptySpaceStartIndex++; // 아이템 인벤토리에 Strawberry를 추가한 뒤  그 Strawberry의 개수를 1개 추가
        //    }
        //}
        ItemManager.Instance.itemInventory.AddItem(ItemType.Strawberry, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Avocado, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Peanut, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Firewood, 90);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Gazami, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Galchi, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Shark, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Stone, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Iron, 30);
        ItemManager.Instance.itemInventory.AddItem(ItemType.Gold, 30);
        //itemInventoryWindow.RefreshItemInventory();
        Debug.Log(ItemManager.Instance.itemInventory.ItemAmountArray[0]); // 아이템 인벤토리 0번째 칸에 있는 아이템의 개수 출력
        Debug.Log(ItemManager.Instance[ItemType.Strawberry].ItemName); // Strawberry의 (한글)이름 출력
        Debug.Log(ItemManager.Instance[ItemType.Strawberry].Explan); // Strawberry의 설명 출력
        Debug.Log(((FoodItemData)(ItemManager.Instance[ItemType.Strawberry])).AmountOfHungerRecovery); // Strawberry의 허기회복량 출력
        //Debug.Log(((ToolItemData)(ItemManager.Instance[ItemType.IronAxe])).Level); 
    }

    protected override void DoAction5(InputAction.CallbackContext _)
    {
        ItemManager.Instance.itemInventory.AddItem(ItemType.StoneFishingrod, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.IronFishingrod, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.GoldFishingrod, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.StoneAxe, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.IronAxe, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.GoldAxe, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.StonePickaxe, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.IronPickaxe, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.GoldPickaxe, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.StoneSickle, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.IronSickle, 1);
        ItemManager.Instance.itemInventory.AddItem(ItemType.GoldSickle, 1);
        //Debug.Log($"Me:{ItemManager.Instance.itemInventory.FindItem(ItemType.Avocado, 70)}");
        //SceneManager.LoadScene("SampleScene2");
    }
}
