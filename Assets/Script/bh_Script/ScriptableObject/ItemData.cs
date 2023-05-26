using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTag
{
    Food = 0,
    Material,
    Tool,
    Etc,
    Deployment
}

[CreateAssetMenu(menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{

    [SerializeField]
    protected string itemName;
    public string ItemName { get => itemName; }
    [SerializeField]
    protected string explan;
    public string Explan { get => explan; }
    [SerializeField]
    protected ItemTag tag;
    public ItemTag Tag { get => tag; }
    [SerializeField]
    protected Sprite iconSprite;
    public Sprite IconSprite { get => iconSprite; }
    [SerializeField]
    protected List<ItemType> productionMaterialTypeList;
    public List<ItemType> ProductionMaterialTypeList { get => productionMaterialTypeList; }
    [SerializeField]
    protected List<int> productionMaterialAmountList;
    public List<int> ProductionMaterialAmountList { get => productionMaterialAmountList; }
}
