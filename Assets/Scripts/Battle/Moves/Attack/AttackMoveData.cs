using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class AttackMoveData : MoveData
{
    public float Damage;

    public override async Task Execute(Unit unitExecutor)
    {
        await base.Execute(unitExecutor);

        // float damageToApplay = unitExecutor.Attack * Damage / 100f;

        float damageToApply = (((2 * unitExecutor.Level / 5) + 2) * Damage * (unitExecutor.Attack / unitExecutor.Enemy.Defence) / 50) + 2;
        unitExecutor.Enemy.TakeDamage(damageToApply);

        Debug.LogError($"{unitExecutor.Name} Usou {Name} em {unitExecutor.Enemy.Name} inimigo e causou {damageToApply} pontos de dano");
    }
}