using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum UnitType
{
    Player,
    Enemy
}

public enum UnitAttribute
{
    None,
    Attack,
    Speed
}

public class Unit : MonoBehaviour
{
    public event Action<float> OnDamaged;

    public int Level => _level;

    [field: SerializeField] public UnitType Type { get; private set; }
    [field: SerializeField] public Unit Enemy { get; set; }

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float CurrentHealth { get; private set; }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Defence { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public List<MoveData> Moves { get; private set; }

    [SerializeField] private int _level = 1;
    [SerializeField] private UnitData _unitData;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Name = _unitData.Name;
        Attack = _unitData.Attack;
        Defence = _unitData.Defence;
        Speed = _unitData.Speed;
        Moves = _unitData.Moves;

        MaxHealth = _unitData.Health;
        CurrentHealth = MaxHealth;
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
    /// <param name="modifier">Grau do modificador entre -5 a 5</param>
    /// </summary>
    public void ApplyAttackModifier(int modifierDegree)
    {
        float modifier = .2f * modifierDegree;
        Attack = Mathf.Clamp(Attack + (_unitData.Attack * modifier), 1, _unitData.Attack * 2f);
    }

    public void ApplySpeedModifier(int modifierDegree)
    {
        float modifier = .2f * modifierDegree;
        Speed = Mathf.Clamp(Speed + (_unitData.Speed * modifier), 1, _unitData.Speed * 2f);
    }
}