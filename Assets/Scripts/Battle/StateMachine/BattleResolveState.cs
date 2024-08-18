using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BattleResolveState : BattleStateBase
{
    private List<Unit> _unitsInBattle;
    private GameObject _ui;
    private List<RoundMove> _movesChosen;
    private bool _isResolvingTurn;

    public BattleResolveState(BattleManager stateMachine, List<Unit> unitsInBattle, GameObject ui, List<RoundMove> roundMovesChosen) : base(stateMachine)
    {
        _unitsInBattle = unitsInBattle;
        _ui = ui;

        _movesChosen = roundMovesChosen;
        _movesChosen.Clear();

        _isResolvingTurn = false;
    }

    public override void OnEnter()
    {
        _ui.SetActive(true);
        Unit enemy = _unitsInBattle.Find(unit => unit.Type == UnitType.Enemy);
        _movesChosen.Add(new() { Type = UnitType.Enemy, Move = enemy.Moves.GetRandom() });
    }

    public override void OnTick(float deltaTime)
    {
        if (_movesChosen.Count == 2 && !_isResolvingTurn)
        {
            ResolveTurn();
        }
    }

    public override void OnExit()
    {
    }

    private async void ResolveTurn()
    {
        Debug.Log("Took Turn");

        _isResolvingTurn = true;
        _ui.SetActive(false);

        foreach (Unit unit in _unitsInBattle)
        {
            _movesChosen.Find(roundMove => roundMove.Type == unit.Type).Move.Execute(unit);
            await Task.Delay(1000);
        }

        stateMachine.SwitchState(new BattleVerifyState(stateMachine, _unitsInBattle));
    }
}
