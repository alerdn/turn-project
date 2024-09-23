using UnityEngine;

public class BattleEnemyTurnState : BattleBaseState
{
    private float _time;
    private EnemyController _enemyController;
    private PlayerController _playerController;
    private BattleInteraction _interactionUI;

    public BattleEnemyTurnState(BattleStateMachine stateMachine, EnemyController enemyController, PlayerController playerController, BattleInteraction battleInteraction) : base(stateMachine)
    {
        _enemyController = enemyController;
        _playerController = playerController;
        _interactionUI = battleInteraction;
    }

    public override void OnEnter()
    {
        _playerController.InputReader.EnableDefensiveInputs();

        _time = Time.time;

        ResolveTurn();
    }

    private async void ResolveTurn()
    {
        MoveData move = _enemyController.Unit.ChoseMove();
        _interactionUI.Init(_enemyController.Unit, move, _time);

        await move.Execute(_enemyController.Unit);

        // Verificamos se algum dos personagens foi derrotado
        if (stateMachine.VerifyBattleFinished(_playerController, _enemyController, out Unit defeatedUnit))
        {
            stateMachine.SwitchState(new BattleEndState(stateMachine, defeatedUnit));
            return;
        }

        stateMachine.StartPlayerTurn();
    }

    public override void OnTick(float deltaTime)
    {
        _time += deltaTime;

        if (_interactionUI.IsInteractable)
        {
            _interactionUI.UpdateState(_time);

            InputReader input = PlayerController.Instance.InputReader;
            if (input.Controls.Defensive.Interact.WasPerformedThisFrame())
            {
                _interactionUI.TryInteract(_time);
            }
        }
    }

    public override void OnExit()
    {
        _playerController.InputReader.DisableDefensiveInputs();
    }
}
