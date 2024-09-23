using System;
using UnityEngine;

public class PlayerCombatComponent : MonoBehaviour
{
    [SerializeField] private float _power = 100f;

    public void ApplyDamage(Unit target, MoveType type)
    {
        float damageToApply = GetDamageToApply(target, type);
        target.TakeDamage(damageToApply);
        Debug.Log($"Player causou {damageToApply} de dano ao inimigo");
    }

    private (float, float) GetStatskByType(Unit unit, MoveType type)
    {
        return type switch
        {
            MoveType.Physical => (unit.Attack, unit.Enemy.Defence),
            MoveType.Special => (unit.SpecialAttack, unit.Enemy.SpecialDefence),
            _ => throw new NotImplementedException()
        };
    }

    private float GetDamageToApply(Unit unitExecutor, MoveType type)
    {
        // Será usado posteriormente
        float modifier = 1f;

        (float attackStat, float enemyDefenceStat) = GetStatskByType(unitExecutor, type);
        float damageToApply = ((((2 * unitExecutor.Level / 5) + 2) * _power * (attackStat / enemyDefenceStat) / 50) + 2) * modifier;

        return damageToApply;
    }
}