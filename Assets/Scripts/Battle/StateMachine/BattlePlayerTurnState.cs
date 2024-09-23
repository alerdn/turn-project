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
    private bool _hasPerformedAction;

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
        _hasPerformedAction = false;

        _playerController.InputReader.EnableOffensiveInputs();
        _playerController.InputReader.AttackEvent += OnAttack;
        _playerController.InputReader.ToggleTurnModeEvent += OnToggleTurnMode;

        _battleMenu.MoveMenu.OnSelectAction += OnSelectAction;
        _battleMenu.ItemMenu.OnSelectAction += OnSelectAction;
    }

    public override void OnTick(float deltaTime)
    {
        _time += deltaTime;

        if ((_time > _actionEndTime && !_isPerformingAction && !_isTurnMode) || _hasPerformedAction)
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

        _battleMenu.MoveMenu.OnSelectAction -= OnSelectAction;
        _battleMenu.ItemMenu.OnSelectAction -= OnSelectAction;
    }

    private void OnAttack()
    {
        if (_isAttacking || _isTurnMode) return;

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
            _battleMenu.ShowMenu();
        }
        else
        {
            _battleMenu.HideMenu();
        }
    }

    private void OnSelectAction(ActionData action)
    {
        _isTurnMode = false;
        _playerController.InputReader.DisableOffensiveInputs();
        _battleMenu.HideMenu();

        HandleAction(action);
    }

    private async void HandleAction(ActionData action)
    {
        _isPerformingAction = true;
        await action.Execute(_playerController.Unit);

        _isPerformingAction = false;
        _hasPerformedAction = true;
    }
}
