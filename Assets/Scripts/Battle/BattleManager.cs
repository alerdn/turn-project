using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RoundMove
{
    public UnitType Type;
    public MoveData Move;
}

public class BattleManager : StateMachine
{
    public static BattleManager Instance { get; private set; }

    public event Action OnBattleEnded;

    public Unit CurrentUnit { get; private set; }
    public List<RoundMove> RoundMovesChosen => _roundMovesChosen;

    [Header("UI")]
    [SerializeField] private MainBattleMenu _battleUI;
    [SerializeField] private BattleMenu _playerMainManu;
    [SerializeField] private FightBattleMenu _playerFightMenu;
    [SerializeField] private StatusUI _playerStatus;
    [SerializeField] private StatusUI _enemyStatus;


    [Header("Debug")]
    [SerializeField] private List<Unit> _unitsInBattle;
    [SerializeField] private List<RoundMove> _roundMovesChosen = new();

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
        _battleUI.HideMenu();
    }

    public void StartBattle(PlayerController playerController, Vector2 playerPosition, Unit enemy, Vector2 enemyPosition)
    {
        ShowBattleUI();

        playerController.InputReader.DisableInputs();
        playerController.transform.SetPositionAndRotation(playerPosition, Quaternion.identity);

        Unit player = playerController.PlayerUnit;
        player.ResetStats();
        _playerStatus.Init(player);
        _playerFightMenu.Init(player);

        _enemyStatus.Init(enemy);
        enemy.transform.SetPositionAndRotation(enemyPosition, Quaternion.identity);
        enemy.Init();

        _unitsInBattle = new() { player, enemy };

        _unitsInBattle[0].Enemy = _unitsInBattle[1];
        _unitsInBattle[1].Enemy = _unitsInBattle[0];

        NextRound();
    }

    public void NextRound()
    {
        SwitchState(new BattleResolveState(this, _unitsInBattle, _playerMainManu, _roundMovesChosen));
    }

    public void EndBattle()
    {
        OnBattleEnded?.Invoke();
    }

    public void ShowBattleUI()
    {
        _battleUI.ShowMenu();
    }

    public void HideBattleUI()
    {
        _battleUI.HideMenu();
    }
}