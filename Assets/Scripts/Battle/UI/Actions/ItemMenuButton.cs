
using UnityEngine;

public class ItemMenuButton : BaseMenuButton
{
    private ItemStock _stock;
    private ItemBattleMenu _itemBattleMenu;

    public void Init(ItemStock stock, ItemBattleMenu itemBattleMenu)
    {
        InitComponents();

        _stock = stock;
        _itemBattleMenu = itemBattleMenu;

        ButtonText.text = _stock.Item.Name;
    }

    public override void Execute()
    {
        if (_stock.Quantity > 0)
        {
            _itemBattleMenu.SelectAction(_stock.Item);
            _stock.Quantity--;
        }
        else
        {
            Debug.Log("Item não disponível");
        }
    }
}