using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemManager 
{
    private string item;
    private bool hasItem;
    private int totalItem;
    private bool isSingular;
    private float price;

    public string Item { get => item; set => item = value; }
    public bool HasItem { get => hasItem; set => hasItem = value; }
    public int TotalItem { get => totalItem; set => totalItem = value; }
    public bool IsSingular { get => isSingular; set => isSingular = value; }
    public float Price { get => price; set => price = value; }

    public ItemManager(string item, bool hasItem, int totalItem, bool isSingular, float price)
    {
        this.Item = item;
        this.HasItem = hasItem;
        this.TotalItem = totalItem;
        this.IsSingular = isSingular;
        this.price = price;
    }
}
