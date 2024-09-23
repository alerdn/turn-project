using System;
using System.Threading.Tasks;
using UnityEngine;

public class BattlePlayerTurnState : BattleBaseState
{
    private float _time;
    private float _actionEndTime;
    private PlayerController _playerController;
    private EnemyController _enemyController;
    private bool _isAttacking;

    public BattlePlayerTurnState(BattleStateMachine stateMachine, PlayerController playerController, EnemyController enemyController) : base(stateMachine)
    {
        _playerController = playerController;
        _enemyController = enemyController;
    }

    public override void OnEnter()
    {
        _time = Time.time;
        
        _actionEndTime = _time + _playerController.Unit.ActionDuration;
        _isAttacking = false;

        _playerController.InputReader.EnableOffensiveInputs();
        _playerController.InputReader.AttackEvent += OnAttack;
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
    }

    private void OnAttack()
    {
        if (_isAttacking) return;

        HandleAttack();
    }

    private async void HandleAttack()
    {
        _isAttacking = true;

        _playerController.CombatComponent.ApplyDamage(_enemyController.Unit, MoveType.Physical);        
        await Task.Delay(200);
        
        _isAttacking = false;
    }
}
