using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBase : MonoBehaviour
{
    // ����ð� 6�� = ���ӽð� 12�ð� / 1�� - 2�ð� / 30�� ���� hp 1 ����
    [Header("�÷��̾� ������")]
    //--------------------Public---------------------------
    public float moveSpeed = 5.0f;      //�̵��ӵ�
    public float turnSpeed = 0.5f;     //ȸ���ӵ�
    public float rushSpeed = 10.0f;
    //--------------------private---------------------------
    private float mouseDelta;          // ���콺�� ��ġ��
    private int maxHp = 1000;          // �ִ� hp
    private int hp = 1000;              // ���� hp

    public bool isAction = false;
    private bool isRun = false;
    private bool isDead = false;

    private bool isDoing = false;

    public bool ifCraft = false;
    private bool isMove = true;

    public int HP                      // ���� hp ������Ƽ > ui
    {
        get => hp;
        set
        {
            if (value > 1000)
            {
                hp = maxHp;
            }
            else if(1<value && value< 1000)
            {
                hp = value;
                Debug.Log(hp);
            }
            else if (value < 2 && isDead==false)
            {
                isDead = true;
                hp = 0;
                OnDie();
            }

            onUpgradeHp?.Invoke(hp / maxHp);
        }
    }

    public Action<float> onUpgradeHp;
    public Action onDie;
    public Action onMaking;
    public Action onInventory;

    /*------------------����ó�� -------------------*/

    /*------------------�÷��̾� ���� -------------------*/
    public enum playerState
    {
        Nomal,
        TreeFelling,    //����
        Gathering,      //Ǯä��
        Mining,         //ä��
        Fishing,        //����
    }


    playerState state;
    public Action<playerState> GetState;
    public playerState State
    {
        get => state;
        set => state = value;
    }
    /*------------------ToolItem ���� -------------------*/
    public bool[] isEqualWithState = new bool[5];   //playerState enum �������
    public bool[] IsdEqualState
    {
        get => isEqualWithState;
    }

    private GameObject[] tools;                     // axe, Reap, Pick, FishingRod �������
    private string[] toolsNames = { "Axe", "Reap", "Pick", "FishingRod" };

    public Action<ToolItemTag, int> GetToolItem;    //���� ������ ���� ��������Ʈ

    private int playercurrentToolItem;
    private int playertoolLevel;

    //------------------------------��Ÿ----------------------------------------------
    [Header("������Ʈ")]
    private Animator anim;
    private Rigidbody rigid;

    private ItemInventoryWindowExplanRoom item;

    SaveBoardUI pauseMenu;

    private Axe axe;
    private FishinfRod fishingRod;
    private Reap reap;
    private Pick pick;
    private RightHand rHand;

    private CraftingWindow craft;

    [Header("�Է� ó����")]
    private PlayerInput inputActions; 
    private Vector3 inputDir = Vector3.zero;
    Vector3 V3;

    //----------------------------------�Ϲ� �Լ�-------------------------------
    private void Awake()
    {
        pauseMenu = FindObjectOfType<SaveBoardUI>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        inputActions = new PlayerInput();
        axe = FindObjectOfType<Axe>();
        fishingRod = FindObjectOfType<FishinfRod>();
        reap = FindObjectOfType<Reap>();
        pick = FindObjectOfType<Pick>();
        rHand = FindObjectOfType<RightHand>();
        craft = FindObjectOfType<CraftingWindow>();
        item = FindObjectOfType<ItemInventoryWindowExplanRoom>();
        if (item == null)
        {
            item = ItemManager.Instance.itemInventory.ItemsInventoryWindow.ExplanRoom;
        }
    }
    private void OnEnable()
    {
        //�ִϸ��̼� �׽�Ʈ��

        /*inputActions.Test.Enable();
        inputActions.Test.Test1.performed += Test1;*/

        //-----�Է� Enable----
        inputActions.CharacterMove.Enable();
        inputActions.OpenWindow.Enable();
        inputActions.CharacterMove.MouseMove.performed += OnMouseMoveInput;
        inputActions.CharacterMove.Move.performed += OnMoveInput;
        inputActions.CharacterMove.Move.canceled += OnMoveInput;
        inputActions.CharacterMove.Rush.performed += OnRunInput;
        inputActions.CharacterMove.Rush.canceled += OnRunInputStop;

        inputActions.CharacterMove.Activity.performed += OnAvtivity;
        inputActions.CharacterMove.Activity.canceled += OnAvtivityStop;

        inputActions.CharacterMove.Interaction_Item.performed += OnGrab;

        inputActions.OpenWindow.OpenItemWindow.performed += Oninventory;
        inputActions.OpenWindow.OpenCraftWindow.performed += OnMaking;

        //-----���� ���� ��������Ʈ----
        pauseMenu.updateData += SetData;
        item.onChangeHp += OnUpgradeHp; // <<�κ�
        item.onChangeTool += OnUpgradeTool;

        axe.UsingTool += OnUpgradeHp;
        reap.UsingTool += OnUpgradeHp;
        pick.UsingTool += OnUpgradeHp;
        fishingRod.UsingTool += OnUpgradeHp;
        rHand.UsingTool += OnUpgradeHp;
        craft.DoAction += CraftDoAction;
        craft.DontAction += CraftDontAction;
    }

    private void OnDisable()
    {
        //�ִϸ��̼� �׽�Ʈ��

        /*inputActions.Test.Test1.performed -= Test1;
        inputActions.Test.Disable();*/
        //-----�Է� Disable----
        inputActions.CharacterMove.MouseMove.performed -= OnMouseMoveInput;
        inputActions.CharacterMove.Move.canceled -= OnMoveInput;
        inputActions.CharacterMove.Move.performed -= OnMoveInput;
        inputActions.CharacterMove.Rush.performed -= OnRunInput;
        inputActions.CharacterMove.Rush.canceled -= OnRunInputStop;

        inputActions.CharacterMove.Activity.performed -= OnAvtivity;
        inputActions.CharacterMove.Activity.canceled -= OnAvtivityStop;

        inputActions.CharacterMove.Interaction_Item.performed -= OnGrab;
        inputActions.CharacterMove.Disable();
        inputActions.OpenWindow.Disable();
    }

   

    private void Start()
    {
        /*item = FindObjectOfType<ItemInventoryWindowExplanRoom>();
        if (item == null)
        {
            item = ItemManager.Instance.itemInventory.ItemsInventoryWindow.ExplanRoom;
        }

        axe = FindObjectOfType<Axe>();
        fishingRod = FindObjectOfType<FishinfRod>();
        reap = FindObjectOfType<Reap>();
        pick = FindObjectOfType<Pick>();
        rHand = FindObjectOfType<RightHand>();
        craft = FindObjectOfType<CraftingWindow>();

        pauseMenu.updateData += SetData;
        item.onChangeHp += OnUpgradeHp; // <<�κ�
        item.onChangeTool += OnUpgradeTool;

        axe.UsingTool += OnUpgradeHp;
        reap.UsingTool += OnUpgradeHp;
        pick.UsingTool += OnUpgradeHp;
        fishingRod.UsingTool += OnUpgradeHp;
        rHand.UsingTool += OnUpgradeHp;
        craft.DoAction += CraftDoAction;
        craft.DontAction += CraftDontAction;*/

        HP = maxHp;
        HpChange();

        if(DataController.Instance.WasSaved == false)
        {
            PreInitialize();
        }
        else
        {
            Initialize();
        }
    }

    public void SetData()
    {
        DataController.Instance.gameData.playerPosition = transform.position;
        DataController.Instance.gameData.playerHp = HP;
        DataController.Instance.gameData.currentToolItem = playercurrentToolItem;
        DataController.Instance.gameData.toolLevel = playertoolLevel;

        DataController.Instance.gameData.toolType = ItemManager.Instance.itemInventory._equipToolIndex;
        DataController.Instance.gameData.itemCount = ItemManager.Instance.itemInventory.ItemAmountArray;
        DataController.Instance.gameData.itemTypes = ItemManager.Instance.itemInventory.ItemTypeArray;
        DataController.Instance.gameData.workbenchPosition = ItemManager.Instance.SetUpItemPosition;
    }

   private void PreInitialize()
    {
        isAction = false;
        isRun = false;
        isDead = false;
        isDoing = false;
        ifCraft = false;

        tools = new GameObject[toolsNames.Length];
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i] = GameObject.Find(toolsNames[i]);
            tools[i].SetActive(false);
        }

        for (int i = 0; i < isEqualWithState.Length; i++)
        {
            isEqualWithState[i] = false;
        }
        isEqualWithState[0] = true;
        state = playerState.Nomal;

        transform.position = Vector3.zero;
        HP = maxHp;
        playercurrentToolItem = int.MinValue;
        playertoolLevel = int.MinValue;

        /*transform.position = DataController.Instance.gameData.playerPosition;
        HP = DataController.Instance.gameData.playerHp;
        playercurrentToolItem = DataController.Instance.gameData.currentToolItem;
        playertoolLevel = DataController.Instance.gameData.toolLevel;*/
        ItemManager.Instance.itemInventory.ItemAmountArray = new int[ItemManager.Instance.itemInventoryMaxSpace];
        ItemManager.Instance.itemInventory.ItemTypeArray = new ItemType[ItemManager.Instance.itemInventoryMaxSpace];
        ItemManager.Instance.itemInventory._equipToolIndex = new int[System.Enum.GetValues(typeof(ToolItemTag)).Length];

        for (int i = 0; i < ItemManager.Instance.itemInventoryMaxSpace; i++)
        {
            ItemManager.Instance.itemInventory.ItemTypeArray[i] = ItemType.Null;
        }
        for (int i = 0; i < ItemManager.Instance.itemInventory._equipToolIndex.Length; i++)
        {
            ItemManager.Instance.itemInventory._equipToolIndex[i] = -1;
        }
        ItemManager.Instance.SetUpItemPosition = new Vector3(-0.33f, 0.01f, -3.69f);
    }

    private void Initialize()
    {
        isAction = false;
        isRun = false;
        isDead = false;
        isDoing = false;
        ifCraft = false;

        tools = new GameObject[toolsNames.Length];
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i] = GameObject.Find(toolsNames[i]);
            tools[i].SetActive(false);
        }

        for (int i = 0; i < isEqualWithState.Length; i++)
        {
            isEqualWithState[i] = false;
        }
        isEqualWithState[0] = true;
        state = playerState.Nomal;

        transform.position = DataController.Instance.gameData.playerPosition;
        HP = DataController.Instance.gameData.playerHp;
        playercurrentToolItem = DataController.Instance.gameData.currentToolItem;
        playertoolLevel = DataController.Instance.gameData.toolLevel;

        ItemManager.Instance.itemInventory._equipToolIndex = DataController.Instance.gameData.toolType;
        ItemManager.Instance.itemInventory.ItemAmountArray = DataController.Instance.gameData.itemCount;
        ItemManager.Instance.itemInventory.ItemTypeArray = DataController.Instance.gameData.itemTypes;

        ItemManager.Instance.SetUpItemPosition = DataController.Instance.gameData.workbenchPosition;
        OnUpgradeTool((ToolItemTag)playercurrentToolItem, playertoolLevel);
    }



    private void FixedUpdate()
    {
        Move();
    }

    //----------------------------------��ǲ�� �Լ�-------------------------------
    private void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector3 input = context.ReadValue<Vector2>();   // ���� Ű���� �Է� ��Ȳ �ޱ�
        inputDir.z = input.y;
        inputDir.x = input.x;
        anim.SetBool("IsMove", !context.canceled);
    }
    private void OnMouseMoveInput(InputAction.CallbackContext context)      //���콺 x��ǥ �̵� delta�� �����ϱ�
    {
        mouseDelta = context.ReadValue<float>();
    }

    private void OnRunInput(InputAction.CallbackContext obj)
    {
        isRun = true;
        anim.SetBool("isRun", isRun);
    }
    private void OnRunInputStop(InputAction.CallbackContext obj)
    {
        isRun = false;
        anim.SetBool("isRun", isRun);
    }
    void Move()
    {
        if (isMove == true)
        {
            V3 = new Vector3(0, mouseDelta, 0);      //���콺 z��ǥ �̵� y�� ȸ�� ������ ����
            mouseDelta = 0.0f;                       //�ʱ��� �۾�
            transform.Rotate(V3 * turnSpeed);        // �Ͻ��ǵ� �ӵ���ŭ ȸ��
            if (isRun == true)
            {
                rigid.MovePosition(Time.fixedDeltaTime * rushSpeed * transform.TransformDirection(inputDir).normalized + transform.position);
            }
            else
            {
                rigid.MovePosition(Time.fixedDeltaTime * moveSpeed * transform.TransformDirection(inputDir).normalized + transform.position);       // ����(���α��� �޿����Ʒ�) ���� �̵�ó��
            }
        }
    }

    //----------------------------------Hp ������ �Լ�-------------------------------


    void OnUpgradeHp(int getHp)             //�κ����� ���޹��� hp(getHp)
    {
        HP = HP + getHp;
    }
    void HpChange()
    {
       StartCoroutine(Decrease());
    }

    IEnumerator Decrease()
    {
            while (HP > -1 && isDead==false)
            {
                yield return new WaitForSeconds(1.0f);  //test
                HP--;
            }
    }

    private void OnDie()
    {
        StopCoroutine(Decrease());
        inputActions.CharacterMove.Disable();
        anim.SetTrigger("IsDead");
        onDie?.Invoke();
    }

    //--- ������ ��� �� �Լ�(left click)---
    private void OnAvtivity(InputAction.CallbackContext context)
    {
        if (isAction)
        {
            if ((state == playerState.Fishing) && (isEqualWithState[4] == true))
            {
                StartCoroutine(Fishing());
            }
            else
            {
                if (isDoing == false)
                    StartCoroutine(ActionCoroutine());
            }
        }
    }
    private void OnAvtivityStop(InputAction.CallbackContext context)
    {
        isDoing = false;
        /*StopCoroutine(ActionCoroutine());*/
        StopCoroutine(Fishing());
    }

    //---���ݿ� �ڷ�ƾ---
    IEnumerator ActionCoroutine()
    {
        isDoing = true;
        while (isDoing)    //������ ���������� ���� ��� ���ϱ�
        {
            switch (state)
            {
                case playerState.TreeFelling:
                    if ((isEqualWithState[1] == true))
                    {
                        anim.SetTrigger("Axe_Trigger");
                    }
                    else
                    {
                        anim.SetTrigger("Hand_Trigger");
                    }
                    break;
                case playerState.Gathering:
                    if ((isEqualWithState[2] == true))
                    {
                        anim.SetTrigger("Reap_Trigger");
                    }
                    else
                    {
                        anim.SetTrigger("Hand_Trigger");
                    }
                    break;
                case playerState.Mining:
                    if ((isEqualWithState[3] == true))
                    {
                        anim.SetTrigger("Pick_Trigger");
                    }
                    else
                    {
                        anim.SetTrigger("Hand_Trigger");
                    }
                    break;
                case playerState.Fishing:
                    if (isEqualWithState[4] == false)
                    {
                        anim.SetTrigger("Hand_Trigger");
                    }
                    break;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator Fishing()
    {
        anim.SetBool("IsFishing", true);
        yield return new WaitForSeconds(5.0f);
        anim.SetBool("IsFishing", false);

    }
    private void OnUpgradeTool(ToolItemTag toolItem, int level)
    {
        isEqualWithState[0] = true;
        switch (toolItem)
        {
            case ToolItemTag.Axe:
                ResetToolState();
                tools[0].SetActive(true);
                axe.OnCangeAxelLevel();
                isEqualWithState[1] = true;
                break;
            case ToolItemTag.Sickle:
                ResetToolState();
                tools[1].SetActive(true);
                reap.OnCangeReapLevel();
                isEqualWithState[2] = true;
                break;
            case ToolItemTag.Pickaxe:
                ResetToolState();
                tools[2].SetActive(true);
                pick.OnCangePickLevel();
                isEqualWithState[3] = true;
                break;
            case ToolItemTag.Fishingrod:
                ResetToolState();
                tools[3].SetActive(true);
                fishingRod.OnCangeFishinfRodlLevel();
                isEqualWithState[4] = true;
                break;
        }
        playercurrentToolItem = (int)toolItem;
        playertoolLevel = level;
        if(level == -1)
        {
            ResetToolState();
            isEqualWithState[0] = true;
        }
    }

    void ResetToolState()
    {
        for (int i = 0; i < tools.Length; i++)
        {
            tools[i].SetActive(false);
        }
        for (int i = 0; i < isEqualWithState.Length; i++)
        {
            isEqualWithState[i] = false;
        }
    }

    //----------------------------------�׷��� �Լ�-------------------------------

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Workbench") && ItemManager.Instance.SetUpAItem.gameObject.activeSelf==true)
        {
            ifCraft = true;
        }
        // �� ������Ʈ�� Ʈ���Ű� ����� ��
        isEqualWithState[0] = true;
        if (other.gameObject.CompareTag("Tree"))
        {
            isAction = true;
            state = playerState.TreeFelling;
            if (isEqualWithState[2] || isEqualWithState[3] || isEqualWithState[4] == true)
            {
                isAction = false;
                Debug.Log("���Ұ� ������");
            }

            //isTreeFelling = true;
        }
        else if (other.gameObject.CompareTag("Flower"))
        {
            isAction = true;
            state = playerState.Gathering;
            if (isEqualWithState[1] || isEqualWithState[3] || isEqualWithState[4] == true)
            {
                isAction = false;
                Debug.Log("���Ұ� ������");
            }
        }
        else if (other.gameObject.CompareTag("Rock"))
        {
            isAction = true;
            state = playerState.Mining;
            if (isEqualWithState[1] || isEqualWithState[2] || isEqualWithState[4] == true)
            {
                isAction = false;
                Debug.Log("���Ұ� ������");
            }
        }
        else if (other.gameObject.CompareTag("Ocean"))
        {
            isAction = true;
            state = playerState.Fishing;
            if (isEqualWithState[1] || isEqualWithState[2] || isEqualWithState[3] == true)
            {
                isAction = false;
                Debug.Log("���Ұ� ������");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Workbench"))
        {
            ifCraft = false;
        }
        if (other.gameObject.CompareTag("Tree")
           || other.gameObject.CompareTag("Flower")
           || other.gameObject.CompareTag("Rock")
           || other.gameObject.CompareTag("Ocean"))
        {
            isAction = false;
        }
    }*/

    private void OnGrab(InputAction.CallbackContext context)
    {
        isMove = false;
        anim.SetTrigger("ItemGrab");
        if(context.canceled)
        {
            isMove = true;
        }
        else
        {
            Invoke("EnableMovement", 1f); // 1�� �ڿ� �̵� �����ϵ��� ����
        }
    }
    void EnableMovement()
    {
        isMove = true;
    }

    //----------------------------------��� ��ȣ�ۿ� �Լ�-------------------------------

    private void OnMaking(InputAction.CallbackContext obj)
    {
        if (ifCraft == true && ItemManager.Instance.SetUpAItem.gameObject.activeSelf == true)
        {
            onMaking?.Invoke();
        }
    }
    private void CraftDontAction()
    {
        inputActions.CharacterMove.Disable();
    }

    private void CraftDoAction()
    {
        inputActions.CharacterMove.Enable();
    }
    private void Oninventory(InputAction.CallbackContext obj)   // iŰ
    {
        StopActionAtInventory();
        onInventory.Invoke();
    }

    private void StopActionAtInventory()
    {
        if (item.gameObject.activeSelf == true)
        {
            inputActions.CharacterMove.Enable();
        }
        else if (item.gameObject.activeSelf == false)
        {
            inputActions.CharacterMove.Disable();
        }
    }
}
