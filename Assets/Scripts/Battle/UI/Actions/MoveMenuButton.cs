using System;
using System.Threading.Tasks;
using UnityEngine;

public class MoveMenuButton : BaseMenuButton
{
    private MoveData _move;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (ButtonText.text == "" && _move)
        {
            ButtonText.text = _move.Name;
            Button.onClick.AddListener(Execute);
        }
    }

    public void Init(MoveData move)
    {
        _move = move;
    }

    private void Execute()
    {
        BattleManager.Instance.RoundMovesChosen.Add(new() { Type = UnitType.Player, Move = _move });
    }
}