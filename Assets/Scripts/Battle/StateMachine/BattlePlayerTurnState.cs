using System;
using System.Threading.Tasks;
using UnityEngine;

public class BattlePlayerTurnState : BattleBaseState
{
    private float _time;
    private float _actionEndTime;
    private PlayerController _playerController;
    private EnemyController _enemyController;
    private MainBattleMenu _battleMenu;
    private bool _isAttacking;
    private bool _isTurnMode;
    private bool _isPerformingAction;

    public BattlePlayerTurnState(BattleStateMachine stateMachine, PlayerController playerController, EnemyController enemyController, MainBattleMenu battleMenu) : base(stateMachine)
    {
        _playerController = playerController;
        _enemyController = enemyController;
        _battleMenu = battleMenu;
    }

    public override void OnEnter()
    {
        _time = Time.time;
        _battleMenu.HideMenu();

        _actionEndTime = _time + _playerController.Unit.ActionDuration;
        _isAttacking = false;
        _isTurnMode = false;
        _isPerformingAction = false;

        _playerController.InputReader.EnableOffensiveInputs();
        _playerController.InputReader.AttackEvent += OnAttack;
        _playerController.InputReader.ToggleTurnModeEvent += OnToggleTurnMode;
        _battleMenu.MoveMenu.OnSelectAction += HandleAction;
    }

    public override void OnTick(float deltaTime)
    {
        _time += deltaTime;

        if (_time > _actionEndTime && !_isPerformingAction)
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
        _battleMenu.HideMenu();
        _playerController.InputReader.DisableOffensiveInputs();
        _playerController.InputReader.AttackEvent -= OnAttack;
        _playerController.InputReader.ToggleTurnModeEvent -= OnToggleTurnMode;
        _battleMenu.MoveMenu.OnSelectAction -= HandleAction;
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
        _playerController.Unit.ChargeEnergy(.25f);
        await Task.Delay(1000);

        _isAttacking = false;
    }

    private void OnToggleTurnMode()
    {
        _isTurnMode = !_isTurnMode;

        if (_isTurnMode)
        {
            Time.timeScale = 0f;
            _battleMenu.ShowMenu();
        }
        else
        {
            Time.timeScale = 1f;
            _battleMenu.HideMenu();
        }
    }

    private async void HandleAction(MoveData move)
    {
        OnToggleTurnMode();

        _isPerformingAction = true;
        _playerController.InputReader.DisableOffensiveInputs();
        await move.Execute(_playerController.Unit);
        _isPerformingAction = false;
    }
}
