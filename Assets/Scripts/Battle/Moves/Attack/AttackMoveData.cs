using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AttackMoveData : MoveData
{
    public float Damage;

    public override async Task<string> Execute(Unit unitExecutor)
    {
        executor = unitExecutor;
        target = executor.Enemy;

        InteractionsData.ForEach(interaction => interaction.HasInteracted = false);
        executor.DecreaseEnergy(EnergyCost);
        executor.PlayAnimation(Name);

        await Task.Delay(Mathf.RoundToInt(ActionDuration * 1000f));

        float damageToApply = GetDamageToApply(executor);
        target.TakeDamage(damageToApply);

        return PrintLog(executor, damageToApply);
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

    private float GetDamageToApply(Unit unitExecutor)
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

    private string PrintLog(Unit unitExecutor, float damageToApply)
    {
        return $"{unitExecutor.Name} Usou {Name} em {unitExecutor.Enemy.Name} e causou {damageToApply} pontos de dano";
    }
}
