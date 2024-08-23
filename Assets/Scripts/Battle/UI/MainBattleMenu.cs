using UnityEngine;

public class MainBattleMenu : BattleMenu
{
    [SerializeField] private BattleMenu _mainMenu;
    [SerializeField] private BattleMenu _moveMenu;

    public override void ShowMenu()
    {
        base.ShowMenu();
        _mainMenu.ShowMenu();
        _moveMenu.HideMenu();
    }
}
