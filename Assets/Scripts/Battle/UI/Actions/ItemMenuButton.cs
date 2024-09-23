
using UnityEngine;

public class ItemMenuButton : BaseMenuButton
{
    private ItemData _item;
    private ItemBattleMenu _itemBattleMenu;

    public void Init(ItemData item, ItemBattleMenu itemBattleMenu)
    {
        InitComponents();

        _item = item;
        _itemBattleMenu = itemBattleMenu;

        ButtonText.text = _item.Name;
    }

    public override void Execute()
    {
        _itemBattleMenu.SelectAction(_item);
    }
}