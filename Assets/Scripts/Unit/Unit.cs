using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum UnitType
{
    Player,
    Enemy
}

public enum UnitStat
{
    None,
    Attack,
    Defence,
    Speed,
    SpecialAttack,
    SpecialDefence
}

public class Unit : MonoBehaviour
{
    public event Action<float> OnDamaged;

    [field: SerializeField] public Unit Enemy { get; set; }

    public int Level => _level;
    public UnitType Type => _type;
    public string Name => _name;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get => _currentHealth; private set => _currentHealth = value; }
    public float Attack => _attack;
    public float SpecialAttack => _specialAttack;
    public float Defence => _defence;
    public float SpecialDefence => _specialDefence;
    public float Speed => _speed;
    public List<MoveData> Moves => _moves;

    [SerializeField] private int _level = 1;
    [SerializeField] private UnitData _unitData;

    [Header("Stats")]
    [SerializeField] private UnitType _type;
    [SerializeField] private string _name;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _attack;
    [SerializeField] private float _specialAttack;
    [SerializeField] private float _defence;
    [SerializeField] private float _specialDefence;
    [SerializeField] private float _speed;
    [SerializeField] private List<MoveData> _moves;

    [Header("Stats Stage")]
    [SerializeField] private int _attackStage;
    [SerializeField] private int _specialAttackStage;
    [SerializeField] private int _defenceStage;
    [SerializeField] private int _specialDefenceStage;
    [SerializeField] private int _speedStage;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _name = _unitData.Name;
        _attack = _unitData.Attack;
        _specialAttack = _unitData.SpecialAttack;
        _defence = _unitData.Defence;
        _specialDefence = _unitData.SpecialDefence;
        _speed = _unitData.Speed;

        _moves = _unitData.Moves;

        _maxHealth = _unitData.Health;
        _currentHealth = MaxHealth;

        _attackStage = 0;
        _specialAttackStage = 0;
        _defenceStage = 0;
        _specialDefenceStage = 0;
        _speedStage = 0;
    }

    public MoveData ChoseMove()
    {
        int movesCount = Moves.Count;
        List<MoveData> _attackMoves = Moves.FindAll(move => move is AttackMoveData);
        List<MoveData> _statusMoves = Moves.FindAll(move => move is StatusMoveData);

        // Movimentos de ataque tem prioridade
        MoveData move = Random.Range(0, 10) <= 7 ? _attackMoves.GetRandom() : _statusMoves.GetRandom();
        return move;
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);

        OnDamaged?.Invoke(GetHealthPercentage());
    }

    public void Defeat()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// <param name="modifierDegree">Grau do modificador entre -6 a 6</param>
    /// </summary>
    public void ApplyStatModifier(UnitStat stat, int modifierDegree)
    {
        ref float statToModify = ref GetStateRef(stat, out float originalStat);
        ref int stateStageToModify = ref GetStateStageRef(stat);

        stateStageToModify = Mathf.Clamp(stateStageToModify + modifierDegree, -6, 6);

        float upperFactor = 2f;
        float bottomFactor = 2f;

        if (stateStageToModify > 0)
        {
            upperFactor += stateStageToModify;
        }
        else if (stateStageToModify < 0)
        {
            bottomFactor -= stateStageToModify;
        }

        float modifier = upperFactor / bottomFactor;
        statToModify = originalStat * modifier;
    }

    private ref float GetStateRef(UnitStat stat, out float originalStat)
    {
        switch (stat)
        {
            case UnitStat.Attack:
                originalStat = _unitData.Attack;
                return ref _attack;
            case UnitStat.SpecialAttack:
                originalStat = _unitData.SpecialAttack;
                return ref _specialAttack;
            case UnitStat.Defence:
                originalStat = _unitData.Defence;
                return ref _defence;
            case UnitStat.SpecialDefence:
                originalStat = _unitData.SpecialDefence;
                return ref _specialDefence;
            case UnitStat.Speed:
                originalStat = _unitData.Speed;
                return ref _speed;
            default: throw new NotImplementedException();
        }
    }

    private ref int GetStateStageRef(UnitStat stat)
    {
        switch (stat)
        {
            case UnitStat.Attack: return ref _attackStage;
            case UnitStat.SpecialAttack: return ref _specialAttackStage;
            case UnitStat.Defence: return ref _defenceStage;
            case UnitStat.SpecialDefence: return ref _specialDefenceStage;
            case UnitStat.Speed: return ref _speedStage;
            default: throw new NotImplementedException();
        }
    }
}