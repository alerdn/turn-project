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

    [SerializeField] private Unit _playerUnit;
    [SerializeField] private List<Unit> _enemyUnits;
    [SerializeField] private GameObject _playerUI;


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

    [Button]
    public void StarBattle()
    {
        _unitsInBattle = new() { _playerUnit, _enemyUnits.GetRandom() };
        _unitsInBattle = _unitsInBattle.OrderByDescending(unit => unit.Speed).ToList();

        _unitsInBattle[0].Enemy = _unitsInBattle[1];
        _unitsInBattle[1].Enemy = _unitsInBattle[0];

        NextRound();
    }

    public void NextRound()
    {
        SwitchState(new BattleResolveState(this, _unitsInBattle, _playerUI, _roundMovesChosen));
    }
}