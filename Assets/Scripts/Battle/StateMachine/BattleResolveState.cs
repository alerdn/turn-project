using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class BattleResolveState : BattleStateBase
{
    private List<Unit> _unitsInBattle;
    private BattleMenu _menu;
    private BattleInteraction _interactionUI;
    private TMP_Text _logs;
    private List<RoundMove> _movesChosen;
    private RoundMove _currentRoundMove;
    private float _time;
    private bool _isResolvingTurn;

    public BattleResolveState(BattleManager stateMachine, List<Unit> unitsInBattle, BattleMenu menu, BattleInteraction interactionUI, TMP_Text logs, List<RoundMove> roundMovesChosen) : base(stateMachine)
    {
        _unitsInBattle = unitsInBattle;
        _menu = menu;
        _interactionUI = interactionUI;
        _logs = logs;

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

            InputReader input = PlayerController.Instance.InputReader;
            if (input.Controls.Battle.Interact.WasPerformedThisFrame())
            {
                int buttonIndex = input.InteractButtonIndex;
                _interactionUI.TryInteract(_time, buttonIndex);
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
            _interactionUI.Init(unit, _currentRoundMove.Move, _time);

            _logs.text = await _currentRoundMove.Move.Execute(unit);

            if (VerifyBattleFinished(out Unit defeatedUnit))
            {
                stateMachine.SwitchState(new BattleEndState(stateMachine, defeatedUnit));
                return;
            }

            await Task.Delay(2000);
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
