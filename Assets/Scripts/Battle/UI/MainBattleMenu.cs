using UnityEngine;

public class MainBattleMenu : BattleMenu
{
    public FightBattleMenu MoveMenu => _moveMenu;

    [SerializeField] private BattleMenu _mainMenu;
    [SerializeField] private FightBattleMenu _moveMenu;

    public override void ShowMenu()
    {
        base.ShowMenu();
        _mainMenu.ShowMenu();
        _moveMenu.HideMenu();
        _moveMenu.Init(PlayerController.Instance.Unit);
    }
}
