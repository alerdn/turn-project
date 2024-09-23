using System;
using UnityEngine;

public class BattleStateMachine : StateMachine
{
    public static BattleStateMachine Instance { get; private set; }

    public event Action OnBattleEnded;

    [Header("UI")]
    [SerializeField] private GameObject _battleFrame;
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

        _playerController = playerController;
        _enemyController = enemyController;

        _playerController.InputReader.DisableMovementInputs();
        _playerController.transform.SetPositionAndRotation(playerPosition, Quaternion.identity);
        Unit player = _playerController.Unit;
        player.ResetStats();
        _playerStatus.Init(player);

        _enemyController.DisableMovement(-1);
        _enemyController.transform.position = enemyPosition;
        Unit enemy = _enemyController.Unit;
        enemy.PlayAnimation("Idle_Battle");
        _enemyStatus.Init(enemy);
        enemy.Init();

        player.Enemy = enemy;
        enemy.Enemy = player;

        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        SwitchState(new BattlePlayerTurnState(this, _playerController, _enemyController));
    }

    public void StartEnemyTurn()
    {
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
