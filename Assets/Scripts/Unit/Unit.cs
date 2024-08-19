using System;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    Player,
    Enemy
}

public class Unit : MonoBehaviour
{
    public event Action<float> OnDamaged;

    [field: SerializeField] public UnitType Type { get; private set; }
    [field: SerializeField] public Unit Enemy { get; set; }

    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public float MaxHealth { get; private set; }
    [field: SerializeField] public float CurrentHealth { get; private set; }
    [field: SerializeField] public float Attack { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public List<MoveData> Moves { get; private set; }

    [SerializeField] private UnitData _unitData;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        Name = _unitData.Name;
        Attack = _unitData.Attack;
        Speed = _unitData.Speed;
        Moves = _unitData.Moves;

        MaxHealth = _unitData.Health;
        CurrentHealth = MaxHealth;
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

    public void ApplyAttackModifier(float modifier)
    {
        Attack = Mathf.Clamp(Attack + (_unitData.Attack * modifier), _unitData.Attack * .5f, _unitData.Attack * 1.5f);
    }

    public void Defeat()
    {
        Destroy(gameObject);
    }
}