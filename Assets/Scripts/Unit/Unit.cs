using System;
using System.Collections.Generic;
using UnityEngine;

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

[RequireComponent(typeof(Animator))]
public class Unit : MonoBehaviour
{
    public event Action<float> OnHealthUpdated;
    public event Action<int> OnEnergyUpdated;
    public event Action<CameraShakeSetting> ImpactEvent;

    public Unit Enemy { get; set; }
    public MoveData LastMoveChosen { get; set; }

    public Animator Animator => _animator;
    public float ActionDuration => _actionDuration;
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
    public int EnergyAmount
    {
        get => _energyAmount; set
        {
            if (value > 6) return;

            _energyAmount = value;
            OnEnergyUpdated?.Invoke(_energyAmount);
        }
    }

    [Header("Components")]
    [SerializeField] private UnitData _unitData;

    [Header("Stats")]
    [SerializeField] private float _actionDuration = 5f;
    [SerializeField] private int _level = 1;
    [SerializeField] private UnitType _type;

    private string _name;
    private float _maxHealth;
    private float _currentHealth;
    private float _attack;
    private float _specialAttack;
    private float _defence;
    private float _specialDefence;
    private float _speed;
    private List<MoveData> _moves;
    private int _energyAmount;
    private float _energyChargingAmount;
    private int _attackStage;
    private int _specialAttackStage;
    private int _defenceStage;
    private int _specialDefenceStage;
    private int _speedStage;

    private Animator _animator;
    private AnimationHelper _animationHelper;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animationHelper = GetComponent<AnimationHelper>();
    }

    private void Start()
    {
        Init();

        _animationHelper.ImpactsEvent += OnImpactEffect;
        _animationHelper.SpawnEffectEvent += OnSpawnEffect;
    }

    private void OnDestroy()
    {
        _animationHelper.ImpactsEvent -= OnImpactEffect;
        _animationHelper.SpawnEffectEvent -= OnSpawnEffect;
    }

    public void Init()
    {
        ResetStats();

        _moves = _unitData.Moves;

        _maxHealth = _unitData.Health;
        _currentHealth = MaxHealth;
    }

    public void ResetStats()
    {
        // Stats
        _name = _unitData.Name;
        _attack = _unitData.Attack;
        _specialAttack = _unitData.SpecialAttack;
        _defence = _unitData.Defence;
        _specialDefence = _unitData.SpecialDefence;
        _speed = _unitData.Speed;

        // Stats stage
        _attackStage = 0;
        _specialAttackStage = 0;
        _defenceStage = 0;
        _specialDefenceStage = 0;
        _speedStage = 0;

        // Energy for use moves
        EnergyAmount = 0;
    }

    #region Combat

    public void PlayAnimation(string moveName)
    {
        string prefix = Type switch
        {
            UnitType.Player => "ANIM_Player_",
            UnitType.Enemy => "ANIM_Enemy_",
            _ => ""
        };

        string animation = prefix + moveName;
        _animator.CrossFadeInFixedTime(animation, .1f);
    }

    public MoveData ChoseMove()
    {
        // int movesCount = Moves.Count;
        // List<MoveData> _attackMoves = Moves.FindAll(move => move is AttackMoveData && move.EnergyCost <= EnergyAmount);
        // List<MoveData> _statusMoves = Moves.FindAll(move => move is StatusMoveData && move.EnergyCost <= EnergyAmount);

        // // Movimentos de ataque tem prioridade
        // MoveData move = Random.Range(0, 10) <= 7 ? _attackMoves.GetRandom() : _statusMoves.GetRandom();
        LastMoveChosen = Moves.FindAll(move => move.EnergyCost <= EnergyAmount).GetRandom();
        return LastMoveChosen;
    }

    #endregion

    #region Health & Energy

    public float GetHealthPercentage()
    {
        return CurrentHealth / MaxHealth;
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);

        OnHealthUpdated?.Invoke(GetHealthPercentage());
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);

        OnHealthUpdated?.Invoke(GetHealthPercentage());
    }

    public void Defeat()
    {
        Destroy(transform.parent.gameObject);
    }

    public void IncreaseEnergy(int amount = 1)
    {
        if (amount < 0) return;

        EnergyAmount += amount;
    }

    public void DecreaseEnergy(int amount)
    {
        if (amount < 0) return;

        EnergyAmount -= amount;
    }

    public void ChargeEnergy(float amount)
    {
        if (amount < 0) return;

        _energyChargingAmount += amount;
        if (_energyChargingAmount >= 1f)
        {
            IncreaseEnergy();
            _energyChargingAmount = 0f;
        }
    }

    #endregion

    #region Stats

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

    #endregion

    #region Events

    private void OnImpactEffect(int index)
    {
        int shakesCount = LastMoveChosen?.ShakeSettings?.Count ?? 0;
        if (shakesCount > 0 && index < shakesCount)
        {
            ImpactEvent?.Invoke(LastMoveChosen.ShakeSettings[index]);
        }
    }

    private void OnSpawnEffect(MoveEffect effectPrefab)
    {
        MoveEffect effect = Instantiate(effectPrefab, transform);
        effect.Init(this);
    }

    #endregion
}