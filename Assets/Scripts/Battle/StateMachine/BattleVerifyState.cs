using System.Collections.Generic;
using UnityEngine;

public class BattleVerifyState : BattleStateBase
{
    private List<Unit> _unitsInBattle;

    public BattleVerifyState(BattleManager stateMachine, List<Unit> unitsInBattle) : base(stateMachine)
    {
        _unitsInBattle = unitsInBattle;
    }

    public override void OnEnter()
    {
        Unit defeatedUnit = _unitsInBattle.Find(unit => unit.CurrentHealth == 0);
        if (defeatedUnit)
        {
            Debug.Log($"{defeatedUnit.Name} foi derrotado. {defeatedUnit.Enemy.Name} venceu");
            stateMachine.SwitchState(null);
        }
        else
        {
            Debug.Log("Começando próxima rodada");
            stateMachine.NextRound();
        }
    }

    public override void OnTick(float deltaTime)
    {
    }

    public override void OnExit()
    {
    }

}