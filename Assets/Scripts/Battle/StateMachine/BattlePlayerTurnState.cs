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
        _isTurnMode = false;
        _isPerformingAction = false;
        _hasPerformedAction = false;

        _playerController.EnterTurnModeEvent += OnEnterTurnMode;

        _battleMenu.MoveMenu.OnSelectAction += OnSelectAction;
        _battleMenu.ItemMenu.OnSelectAction += OnSelectAction;

        _playerController.EnterPlayerTurn();
    }

    public override void OnTick(float deltaTime)
    {
        if (_isTurnMode) return;

        _time += deltaTime;

        if ((_time > _actionEndTime && !_isPerformingAction && !_isTurnMode) || _hasPerformedAction)
        {
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

        _battleMenu.MoveMenu.OnSelectAction -= OnSelectAction;
        _battleMenu.ItemMenu.OnSelectAction -= OnSelectAction;
    }

    private void OnEnterTurnMode(bool turnMode)
    {
        _isTurnMode = turnMode;
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
