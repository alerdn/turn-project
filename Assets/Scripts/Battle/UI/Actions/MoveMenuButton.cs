using System;
using System.Threading.Tasks;
using UnityEngine;

public class MoveMenuButton : BaseMenuButton
{
    private MoveData _move;
    private BattleMenu _fightMenu;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (ButtonText.text == "" && _move)
        {
            ButtonText.text = _move.Name;
            Button.onClick.AddListener(Execute);
        }
    }

    public void Init(MoveData move, BattleMenu fightMenu)
    {
        _move = move;
        _fightMenu = fightMenu;
    }

    private void Execute()
    {
        bool canUseMove = _move.EnergyCost <= PlayerController.Instance.PlayerUnit.EnergyAmount;
        if (canUseMove)
        {
            BattleManager.Instance.RoundMovesChosen.Add(new() { Type = UnitType.Player, Move = _move });
            _fightMenu.HideMenu();
        }
        else
        {
            Debug.LogError("Jogador não possui energia para usar este movimento");
        }
    }
}