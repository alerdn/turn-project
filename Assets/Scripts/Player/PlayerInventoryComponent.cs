using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryComponent : MonoBehaviour
{
    public List<ItemData> Items => _items;

    [SerializeField] private List<ItemData> _items;
}