using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemBattleMenu : BattleMenu
{
    [SerializeField] private Transform _actionParent;

    public void Init(PlayerInventoryComponent inventory)
    {
        List<ItemMenuButton> buttons = _actionParent.GetComponentsInChildren<ItemMenuButton>().ToList();
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            ItemStock stock = inventory.Items[i];
            ItemMenuButton button = buttons[i];

            button.Init(stock, this);
        }
    }
}