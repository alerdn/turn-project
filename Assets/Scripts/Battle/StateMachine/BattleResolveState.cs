using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleResolveState : BattleStateBase
{
    private List<Unit> _unitsInBattle;
    private BattleMenu _menu;
    private List<RoundMove> _movesChosen;
    private bool _isResolvingTurn;
    private bool _isBattleEnded;

    public BattleResolveState(BattleManager stateMachine, List<Unit> unitsInBattle, BattleMenu menu, List<RoundMove> roundMovesChosen) : base(stateMachine)
    {
        _unitsInBattle = unitsInBattle;
        _menu = menu;

        _movesChosen = roundMovesChosen;
        _movesChosen.Clear();

        _isResolvingTurn = false;
        _isBattleEnded = false;
    }

    public override void OnEnter()
    {
        _menu.ShowMenu();

        Unit enemy = _unitsInBattle.Find(unit => unit.Type == UnitType.Enemy);
        enemy.IncreaseEnergy();
        Unit player = _unitsInBattle.Find(unit => unit.Type == UnitType.Player);
        player.IncreaseEnergy();

        _movesChosen.Add(new() { Type = UnitType.Enemy, Move = enemy.ChoseMove() });
    }

    public override void OnTick(float deltaTime)
    {
        if (_movesChosen.Count == 2 && !_isResolvingTurn)
        {
            _isResolvingTurn = true;
            ResolveTurn();
        }
    }

    public override void OnExit()
    {
    }

    private async void ResolveTurn()
    {
        _menu.HideMenu();
        _unitsInBattle = _unitsInBattle.OrderByDescending(unit => unit.Speed).ToList();
        foreach (Unit unit in _unitsInBattle)
        {
            await _movesChosen.Find(roundMove => roundMove.Type == unit.Type).Move.Execute(unit);
            if (VerifyBattleFinished(out Unit defeatedUnit))
            {
                _isBattleEnded = true;
                stateMachine.SwitchState(new BattleEndState(stateMachine, defeatedUnit));
                return;
            }
        }

        stateMachine.NextRound();
    }

    private bool VerifyBattleFinished(out Unit defeatedUnit)
    {
        defeatedUnit = _unitsInBattle.Find(unit => unit.CurrentHealth == 0);
        if (defeatedUnit)
        {
            Debug.Log($"{defeatedUnit.Name} foi derrotado. {defeatedUnit.Enemy.Name} venceu");
            defeatedUnit.Defeat();
            return true;
        }

        return false;
    }
}
