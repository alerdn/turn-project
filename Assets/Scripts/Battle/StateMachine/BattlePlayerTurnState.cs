using UnityEngine;

public class BattlePlayerTurnState : BattleBaseState
{
    private float _time;
    private float _actionEndTime;
    private PlayerController _playerController;
    private EnemyController _enemyController;

    public BattlePlayerTurnState(BattleStateMachine stateMachine, PlayerController playerController, EnemyController enemyController) : base(stateMachine)
    {
        _playerController = playerController;
        _enemyController = enemyController;
    }

    public override void OnEnter()
    {
        _playerController.InputReader.EnableOffensiveInputs();

        _time = Time.time;
        _actionEndTime = _time + _playerController.Unit.ActionDuration;

        Debug.Log("Player Turn");
    }

    public override void OnTick(float deltaTime)
    {
        _time += deltaTime;

        if (_time > _actionEndTime)
        {
            // Verificamos se algum dos personagens foi derrotado
            if (stateMachine.VerifyBattleFinished(_playerController, _enemyController, out Unit defeatedUnit))
            {
                stateMachine.SwitchState(new BattleEndState(stateMachine, defeatedUnit));
                return;
            }

            stateMachine.StartEnemyTurn();
        }
    }

    public override void OnExit()
    {
        _playerController.InputReader.DisableOffensiveInputs();

        Debug.Log("Player Turn Ended");
    }
}
