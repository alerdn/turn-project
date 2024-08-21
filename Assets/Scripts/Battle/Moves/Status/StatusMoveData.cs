using System;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StatusMoveData : MoveData
{
    public UnitAttribute Attribute;
    [Range(-5, 5)]
    public int ModifierDegree;

    public override async Task Execute(Unit unitExecutor)
    {
        await base.Execute(unitExecutor);

        switch (Attribute)
        {
            case UnitAttribute.Attack:
                target.ApplyAttackModifier(ModifierDegree);
                break;
            case UnitAttribute.Speed:
                target.ApplySpeedModifier(ModifierDegree);
                break;

        }

        Debug.LogWarning($"{unitExecutor.Name} usou {Name} em {target.Name} e {(ModifierDegree > 0 ? "aumentou" : "diminuiu")} seu {Attribute}");
    }
}