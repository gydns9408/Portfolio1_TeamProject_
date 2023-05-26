using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInventoryWindowRoom : MonoBehaviour
{
    // Start is called before the first frame update
    Image _PanelImage;
    public Image _panelImage
    {
        get => _PanelImage;
        set => _PanelImage = value;
    }

    [SerializeField]
    int _Index;
    public int _index
    {
        get => _Index;
    }
    Image _itemIcon;
    TextMeshProUGUI _itemAmount;
    Button _selectButton;
    ItemInventoryWindow itemInventoryWindow;

    void Awake()
    {
        _panelImage = GetComponent<Image>();
        Transform child1 = transform.GetChild(0);
        _itemIcon = child1.GetComponent<Image>();
        Transform child2 = transform.GetChild(1);
        _itemAmount = child2.GetComponent<TextMeshProUGUI>();
        Transform child3 = transform.GetChild(2);
        _selectButton = child3.GetComponent<Button>();
        _selectButton.onClick.AddListener(OnSelected);
        Transform parent0 = transform.parent;
        itemInventoryWindow = parent0.GetComponent<ItemInventoryWindow>();
    }

    public void SetSpace(Sprite itemIcon, int itemAmount)
    {
        _itemIcon.enabled = true;
        _itemAmount.enabled = true;
        _selectButton.enabled = true;
        _itemIcon.sprite = itemIcon;
        _itemAmount.text = itemAmount.ToString();
    }

    public void SetSpace(Sprite itemIcon, bool nowEquip)
    {
        _itemIcon.enabled = true;
        _itemAmount.enabled = true;
        _selectButton.enabled = true;
        _itemIcon.sprite = itemIcon;
        if (nowEquip == false)
        {
            _itemAmount.text = "-";
        }
        else
        {
            _itemAmount.text = "E";
        }
    }

    public void DisableComponent()
    {
        _itemIcon.enabled = false;
        _itemAmount.enabled = false;
        _selectButton.enabled = false;
    }

    void OnSelected()
    {
        if (itemInventoryWindow._selectedIndex != _index)
        {
            itemInventoryWindow.SetExplan(_index);
        }
    }

}
