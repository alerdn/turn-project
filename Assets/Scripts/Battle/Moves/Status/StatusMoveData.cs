using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StatusMoveData : MoveData
{
    public UnitStat Stat;
    [Range(-6, 6)]
    public int ModifierDegree;

    public override async Task Execute(Unit unitExecutor)
    {
        await base.Execute(unitExecutor);
        target.ApplyStatModifier(Stat, ModifierDegree);
        Debug.LogWarning($"{unitExecutor.Name} usou {Name} em {target.Name} e {(ModifierDegree > 0 ? "aumentou" : "diminuiu")} seu {Stat}");
    }
}