using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemStock
{
    public ItemData Item;
    public int Quantity;
}

public class PlayerInventoryComponent : MonoBehaviour
{
    public List<ItemStock> Items => _items;

    [SerializeField] private List<ItemStock> _items;
}