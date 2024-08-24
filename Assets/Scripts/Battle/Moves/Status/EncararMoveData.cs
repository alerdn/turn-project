using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EncararMoveData", menuName = "Moves/Status/Encarar")]
public class EncararMoveData : StatusMoveData
{
    public override async Task Execute(Unit unitExecutor)
    {
        HasInteracted = false;
        unitExecutor.DecreaseEnergy(EnergyCost);
        target = unitExecutor;

        await Task.Delay(Mathf.RoundToInt(MoveDuration * 1000f));

        int modifierToApply = GetModifierToApply(unitExecutor);
        target.ApplyStatModifier(Stat, modifierToApply);

        PrintLog(unitExecutor, modifierToApply);
    }
}