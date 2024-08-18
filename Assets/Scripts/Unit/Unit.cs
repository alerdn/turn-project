using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Player,
    Enemy
}

public class Unit : MonoBehaviour
{
    [field: SerializeField] public UnitType Type { get; private set; }
    [field: SerializeField] public Unit Enemy { get; set; }

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float CurrentHealth { get; private set; }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public List<MoveData> Moves { get; private set; }

    [SerializeField] private UnitData _unitData;

    private float _maxHealth;

    private void Start()
    {
        Name = _unitData.Name;
        Attack = _unitData.Attack;
        Speed = _unitData.Speed;
        Moves = _unitData.Moves;

        _maxHealth = _unitData.Health;
        CurrentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;

        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0f);
    }

    public void ApplyAttackModifier(float modifier)
    {
        Attack += _unitData.Attack * modifier;
    }
}