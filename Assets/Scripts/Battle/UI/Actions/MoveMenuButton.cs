using System;
using System.Threading.Tasks;
using UnityEngine;

public class MoveMenuButton : BaseMenuButton
{
    [SerializeField] private MoveData _move;

    protected override void Start()
    {
        base.Start();
        Button.onClick.AddListener(Execute);
        ButtonText.text = _move?.Name ?? "";
    }

    private void Execute()
    {
        BattleManager.Instance.RoundMovesChosen.Add(new() { Type = UnitType.Player, Move = _move });
    }
}