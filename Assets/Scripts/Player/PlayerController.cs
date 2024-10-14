using System;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : ControllerBase
{
    public static PlayerController Instance { get; private set; }

    public event Action<bool> EnterTurnModeEvent;

    public InputReader InputReader => _input;

    [field: SerializeField] public Animator Animator { get; private set; }
    public PlayerCombatComponent CombatComponent { get; private set; }
    public PlayerMovementComponent MovementComponent { get; private set; }
    public PlayerInventoryComponent InventoryComponent { get; private set; }

    [Header("Input")]
    [SerializeField] private InputReader _input;

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

        CombatComponent = GetComponent<PlayerCombatComponent>();
        MovementComponent = GetComponent<PlayerMovementComponent>();
        InventoryComponent = GetComponent<PlayerInventoryComponent>();
    }

    private void Start()
    {
        MovementComponent.Init(_input);
        SwitchState(new PlayerFreelookState(this));
    }

    public void EnterBattleState(Vector2 playerPosition, StatusUI playerStatus, Unit enemy)
    {
        Unit.Enemy = enemy;
        playerStatus.Init(Unit);
        MovementComponent.ResetVelocity();
        transform.SetPositionAndRotation(playerPosition, Quaternion.identity);
        Unit.ResetStats();
        SwitchState(new PlayerWaitingState(this));
    }

    public void EnterFreelookState()
    {
        SwitchState(new PlayerFreelookState(this));
    }

    public void EnterPlayerTurn()
    {
        SwitchState(new PlayerBattleState(this));
    }

    public void NotifyEnterTurnMode()
    {
        EnterTurnModeEvent?.Invoke(true);
    }

    public void NofityExitTurnMode()
    {
        EnterTurnModeEvent?.Invoke(false);
    }
}
