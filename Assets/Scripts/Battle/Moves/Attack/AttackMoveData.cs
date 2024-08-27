using System;
using System.Linq;
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

    public float GetDamageToApply(Unit unitExecutor)
    {
        float modifier = 1f;
        switch (unitExecutor.Type)
        {
            case UnitType.Player:
                if (InteractionsData.Count > 0 && InteractionsData.All(interaction => interaction.HasInteracted))
                {
                    modifier = 2f;
                }
                break;
            case UnitType.Enemy:
                if (InteractionsData.Count > 0 && InteractionsData.All(interaction => interaction.HasInteracted))
                {
                    modifier = .5f;
                }
                break;
        }

        (float attackStat, float enemyDefenceStat) = GetStatskByType(unitExecutor, Type);
        float damageToApply = ((((2 * unitExecutor.Level / 5) + 2) * Damage * (attackStat / enemyDefenceStat) / 50) + 2) * modifier;

        return damageToApply;
    }

    public string PrintLog(Unit unitExecutor, float damageToApply)
    {
        return $"{unitExecutor.Name} Usou {Name} em {unitExecutor.Enemy.Name} e causou {damageToApply} pontos de dano";
    }
}
