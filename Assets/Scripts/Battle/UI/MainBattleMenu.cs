using UnityEngine;

public class MainBattleMenu : BattleMenu
{
    public MoveBattleMenu MoveMenu => _moveMenu;
    public ItemBattleMenu ItemMenu => _itemMenu;

    [SerializeField] private BattleMenu _mainMenu;
    [SerializeField] private MoveBattleMenu _moveMenu;
    [SerializeField] private ItemBattleMenu _itemMenu;

    public void Init(PlayerController playerController)
    {
        _moveMenu.Init(playerController.Unit);
        _itemMenu.Init(playerController.InventoryComponent);
    }

    public override void ShowMenu()
    {
        base.ShowMenu();
        _mainMenu.ShowMenu();
        _moveMenu.HideMenu();
        _itemMenu.HideMenu();
    }
}
