using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Data;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<ObtainableItem> items;
    public Inventory inventory;

    private void Awake()
    {
        Instance = this;
    }
    
    public void AddItem(ObtainableItem item)
    {
        items.Add(item);
        inventory.itemsInventory.Add(item);
        InGameUI.Instance.UpdateItemsUI();
    }
}
