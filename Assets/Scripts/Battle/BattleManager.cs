using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaughtyAttributes;
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

    public Unit CurrentUnit { get; private set; }
    public List<RoundMove> RoundMovesChosen => _roundMovesChosen;

    [Header("Player")]
    [SerializeField] private StatusUI _playerStatus;
    [SerializeField] private FightBattleMenu _playerFightMenu;
    [SerializeField] private GameObject _playerUI;
    [SerializeField] private Unit _playerUnit;
    [Header("Enemy")]
    [SerializeField] private StatusUI _enemyStatus;
    [SerializeField] private Transform _enemiesParent;
    [SerializeField] private List<Unit> _enemyUnitPrefabs;


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
        StartBattle();
    }

    [Button]
    public void StartBattle()
    {
        Unit enemyPrefab = _enemyUnitPrefabs.GetRandom();
        Unit enemy = Instantiate(enemyPrefab, _enemiesParent);
        enemy.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
        enemy.Init();

        _playerStatus.Init(_playerUnit);
        _playerFightMenu.Init(_playerUnit);
        _enemyStatus.Init(enemy);

        _unitsInBattle = new() { _playerUnit, enemy };

        _unitsInBattle[0].Enemy = _unitsInBattle[1];
        _unitsInBattle[1].Enemy = _unitsInBattle[0];

        NextRound();
    }

    public void NextRound()
    {
        SwitchState(new BattleResolveState(this, _unitsInBattle, _playerUI, _roundMovesChosen));
    }
}