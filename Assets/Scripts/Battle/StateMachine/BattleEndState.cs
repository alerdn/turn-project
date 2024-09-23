using System.Threading.Tasks;

public class BattleEndState : BattleBaseState
{
    private Unit _defeatedUnit;

    public BattleEndState(BattleStateMachine stateMachine, Unit defeatedUnit) : base(stateMachine)
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
                PlayerController.Instance.InputReader.EnableMovementInputs();
                break;
        }

        stateMachine.EndBattle();
    }
}
