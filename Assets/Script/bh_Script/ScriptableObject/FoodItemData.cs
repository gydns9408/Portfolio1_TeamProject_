using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AffectStatus
{
    Hp = 0
}

[CreateAssetMenu(menuName = "Scriptable Object/FoodItemData")]
public class FoodItemData : ItemData
{
    // Start is called before the first frame update
    [SerializeField]
    protected int amountOfHungerRecovery;
    public int AmountOfHungerRecovery { get => amountOfHungerRecovery; }

    [SerializeField]
    protected AffectStatus itemsAffectStatus;
    public AffectStatus ItemsAffectStatus { get => itemsAffectStatus; }
}
