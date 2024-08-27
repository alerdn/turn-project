using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleResolveState : BattleStateBase
{
    private List<Unit> _unitsInBattle;
    private BattleMenu _menu;
    private BattleInteraction _interactionUI;
    private List<RoundMove> _movesChosen;
    private RoundMove _currentRoundMove;
    private float _time;
    private bool _isResolvingTurn;

    public BattleResolveState(BattleManager stateMachine, List<Unit> unitsInBattle, BattleMenu menu, BattleInteraction interactionUI, List<RoundMove> roundMovesChosen) : base(stateMachine)
    {
        _unitsInBattle = unitsInBattle;
        _menu = menu;
        _interactionUI = interactionUI;

        _movesChosen = roundMovesChosen;
        _movesChosen.Clear();
        _currentRoundMove = new();

        _isResolvingTurn = false;
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
        _time += Time.deltaTime;

        if (_movesChosen.Count == 2 && !_isResolvingTurn)
        {
            _isResolvingTurn = true;
            ResolveTurn();
        }

        if (_isResolvingTurn && _interactionUI.IsInteractable)
        {
            _interactionUI.UpdateState(_time);

            if (PlayerController.Instance.InputReader.Controls.Battle.Interact.WasPerformedThisFrame())
            {
                _interactionUI.TryInteract(_time);
            }
        }
    }

    public override void OnExit()
    {
    }

    private async void ResolveTurn()
    {
        _menu.HideMenu();
        
        // Sempre ordenar caso as velocidades tenham sido alteradas
        _unitsInBattle = _unitsInBattle.OrderByDescending(unit => unit.Speed).ToList();
        foreach (Unit unit in _unitsInBattle)
        {
            _currentRoundMove = _movesChosen.Find(roundMove => roundMove.Type == unit.Type);
            _interactionUI.Init(_currentRoundMove.Move, _time);

            await _currentRoundMove.Move.Execute(unit);

            if (VerifyBattleFinished(out Unit defeatedUnit))
            {
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
