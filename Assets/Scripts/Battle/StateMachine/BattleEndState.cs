using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleEndState : BattleStateBase
{
    private Unit _defeatedUnit;

    public BattleEndState(BattleManager stateMachine, Unit defeatedUnit) : base(stateMachine)
    {
        _defeatedUnit = defeatedUnit;
    }

    public override void OnEnter()
    {
        stateMachine.HideBattleUI();

        switch (_defeatedUnit.Type)
        {
            case UnitType.Player:
                GameManager.Instance.ReloadLevel();
                break;
            case UnitType.Enemy:
                PlayerController.Instance.InputReader.EnableInputs();
                break;
        }

        stateMachine.EndBattle();
    }

    public override void OnTick(float deltaTime)
    {
    }

    public override void OnExit()
    {
    }
}