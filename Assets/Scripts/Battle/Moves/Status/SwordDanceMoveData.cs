using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordDanceMoveData", menuName = "Moves/Status/Sword Dance")]
public class SwordDanceMoveData : StatusMoveData
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