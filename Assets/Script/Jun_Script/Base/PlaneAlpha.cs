using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneAlpha : MonoBehaviour
{
    ItemManager itemManager;
    public Material mat;

    private void Awake()
    {
        itemManager = FindObjectOfType<ItemManager>();
    }

    private void Start()
    {
        SetActivate(0);
        itemManager.OnHousingmode += ChangeAlpha;
        itemManager.OffHousingmode += ChangeAlphaZero;
    }

    public void ChangeAlpha()
    {
        SetActivate(255);
    }

    public void ChangeAlphaZero()
    {
        SetActivate(0);
    }
    
    void SetActivate(float alpha)
    {
        Color color = mat.color;
        color.a = alpha;
        mat.color = color;
    }

}
