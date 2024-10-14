using System;
using UnityEngine;

public class BattleStateMachine : StateMachine
{
    public static BattleStateMachine Instance { get; private set; }

    public event Action OnBattleEnded;

    [Header("UI")]
    [SerializeField] private GameObject _battleFrame;
    [SerializeField] private MainBattleMenu _battleMenu;
    [SerializeField] private BattleInteraction _interactionUI;
    [SerializeField] private StatusUI _playerStatus;
    [SerializeField] private StatusUI _enemyStatus;

    private PlayerController _playerController;
    private EnemyController _enemyController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideBattleUI();
    }

    public void StartBattle(PlayerController playerController, Vector2 playerPosition, EnemyController enemyController, Vector2 enemyPosition)
    {
        ShowBattleUI();
        _battleMenu.Init(playerController);

        _playerController = playerController;
        _enemyController = enemyController;

        _playerController.EnterBattleState(playerPosition, _playerStatus, _enemyController.Unit);
        _enemyController.EnterBattleState(enemyPosition, _enemyStatus, _playerController.Unit);

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        SwitchState(new BattlePlayerTurnState(this, _playerController, _enemyController, _battleMenu));
    }

    public void StartEnemyTurn()
    {
        _playerController.SwitchState(new PlayerWaitingState(_playerController));
        SwitchState(new BattleEnemyTurnState(this, _enemyController, _playerController, _interactionUI));
    }

    public bool VerifyBattleFinished(ControllerBase player, ControllerBase enemy, out Unit defeatedUnit)
    {
        defeatedUnit = player.Unit.CurrentHealth == 0 ? player.Unit : enemy.Unit.CurrentHealth == 0 ? enemy.Unit : null;
        if (defeatedUnit)
        {
            Debug.Log($"{defeatedUnit.Name} foi derrotado. {defeatedUnit.Enemy.Name} venceu");
            defeatedUnit.Defeat();
            return true;
        }

        return false;
    }

    public void ShowBattleUI()
    {
        _battleFrame.SetActive(true);
        _interactionUI.Hide();
    }

    public void HideBattleUI()
    {
        _battleFrame.SetActive(false);
        _interactionUI.Hide();
    }

    public void EndBattle()
    {
        OnBattleEnded?.Invoke();
    }
}
