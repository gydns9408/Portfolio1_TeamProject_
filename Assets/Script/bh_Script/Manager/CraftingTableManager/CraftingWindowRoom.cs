using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingWindowRoom : MonoBehaviour
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
    Button _selectButton;
    CraftingWindow craftingWindow;

    


    void Awake()
    {
        _panelImage = GetComponent<Image>();
        Transform child1 = transform.GetChild(0);
        _itemIcon = child1.GetComponent<Image>();
        Transform child3 = transform.GetChild(1);
        _selectButton = child3.GetComponent<Button>();
        _selectButton.onClick.AddListener(OnSelected);
        Transform parent0 = transform.parent.parent;
        craftingWindow = parent0.GetComponent<CraftingWindow>();
    }

    public void SetSpace(Sprite itemIcon)
    {
        _itemIcon.enabled = true;
        _selectButton.enabled = true;
        _itemIcon.sprite = itemIcon;
    }



    public void DisableComponent()
    {
        _itemIcon.enabled = false;
        _selectButton.enabled = false;
    }

    void OnSelected()
    {
        if (craftingWindow._selectedIndex != _index)
        {
            craftingWindow.SetExplan(_index);
        }
    }

}
