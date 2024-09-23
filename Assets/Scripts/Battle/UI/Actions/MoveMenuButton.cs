using UnityEngine;

public class MoveMenuButton : BaseMenuButton
{
    private MoveData _move;
    private MoveBattleMenu _moveBattleMenu;

    public void Init(MoveData move, MoveBattleMenu moveBattleMenu)
    {
        InitComponents();

        _move = move;
        _moveBattleMenu = moveBattleMenu;

        ButtonText.text = _move.Name;
    }

    public override void Execute()
    {
        bool canUseMove = _move && _move.EnergyCost <= PlayerController.Instance.Unit.EnergyAmount;
        if (canUseMove)
        {
            PlayerController.Instance.Unit.LastMoveChosen = _move;
            _moveBattleMenu.SelectAction(_move);
        }
        else
        {
            Debug.LogError("Jogador não pode usar este movimento");
        }
    }
}