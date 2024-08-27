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
                if (InteractionsData.Count > 0 && InteractionsData.All(interaction => interaction.HasInteracted))
                {
                    modifier *= 2;
                }
                break;
            case UnitType.Enemy:
                if (InteractionsData.Count > 0 && InteractionsData.All(interaction => interaction.HasInteracted))
                {
                    modifier = Mathf.Max(Mathf.RoundToInt((float)modifier * .5f), 0);
                }
                break;
        }

        return modifier;
    }

    public string PrintLog(Unit unitExecutor, int modifier)
    {
        return $"{unitExecutor.Name} usou {Name} em {target.Name} e {(ModifierDegree > 0 ? "aumentou" : "diminuiu")} seu {Stat} em {modifier}";
    }
}