using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StatusMoveData : MoveData
{
    public UnitStat Stat;
    [Range(-6, 6)]
    public int ModifierDegree;

    public int GetModifierToApply(Unit unitExecutor)
    {
        int modifier = ModifierDegree;
        switch (unitExecutor.Type)
        {
            case UnitType.Player:
                if (InteractionsData.All(interaction => interaction.HasInteracted))
                {
                    modifier *= 2;
                }
                break;
            case UnitType.Enemy:
                if (InteractionsData.All(interaction => interaction.HasInteracted))
                {
                    modifier = Mathf.Max(Mathf.RoundToInt((float)modifier * .5f), 0);
                }
                break;
        }

        return modifier;
    }

    public void PrintLog(Unit unitExecutor, int modifier)
    {
        Debug.LogWarning($"{unitExecutor.Name} usou {Name} em {target.Name} e {(ModifierDegree > 0 ? "aumentou" : "diminuiu")} seu {Stat} em {modifier}");
    }
}