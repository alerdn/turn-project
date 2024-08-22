using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AttackMoveData : MoveData
{
    public float Damage;

    public (float, float) GetStatskByType(Unit unit, MoveType type)
    {
        return type switch
        {
            MoveType.Physical => (unit.Attack, unit.Enemy.Defence),
            MoveType.Special => (unit.SpecialAttack, unit.Enemy.SpecialDefence),
            _ => throw new NotImplementedException()
        };
    }

    public override async Task Execute(Unit unitExecutor)
    {
        await base.Execute(unitExecutor);

        (float attackStat, float enemyDefenceStat) = GetStatskByType(unitExecutor, Type);
        float damageToApply = (((2 * unitExecutor.Level / 5) + 2) * Damage * (attackStat / enemyDefenceStat) / 50) + 2;
        unitExecutor.Enemy.TakeDamage(damageToApply);

        Debug.LogError($"{unitExecutor.Name} Usou {Name} em {unitExecutor.Enemy.Name} inimigo e causou {damageToApply} pontos de dano");
    }
}