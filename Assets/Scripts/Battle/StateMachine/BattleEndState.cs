using System.Threading.Tasks;
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
        EndBattle();
    }

    public override void OnTick(float deltaTime)
    {
    }

    public override void OnExit()
    {
    }

    private async void EndBattle()
    {
        await Task.Delay(2000);
        stateMachine.HideBattleUI();

        switch (_defeatedUnit.Type)
        {
            case UnitType.Player:
                GameManager.Instance.ReloadLevel();
                break;
            case UnitType.Enemy:
                PlayerController.Instance.InputReader.EnablePlayerInputs();
                PlayerController.Instance.InputReader.EnableBattleInputs();
                break;
        }

        stateMachine.EndBattle();
    }
}