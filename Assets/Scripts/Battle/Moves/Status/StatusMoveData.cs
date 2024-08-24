using System;
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
                if (HasInteracted)
                {
                    modifier *= 2;
                    Debug.Log("Acertou o timing do ataque");
                }
                break;
            case UnitType.Enemy:
                if (HasInteracted)
                {
                    modifier = Mathf.Max(Mathf.RoundToInt((float)modifier * .5f), 0);
                    Debug.Log("Acertou o timing da defesa");
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