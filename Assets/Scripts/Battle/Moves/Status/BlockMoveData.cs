using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockMoveData", menuName = "Moves/Status/Block")]
public class BlockMoveData : StatusMoveData
{
    public override async Task<string> Execute(Unit unitExecutor)
    {
        InteractionsData.ForEach(interaction => interaction.HasInteracted = false);
        unitExecutor.DecreaseEnergy(EnergyCost);
        target = unitExecutor;

        await Task.Delay(Mathf.RoundToInt(MoveDuration * 1000f));

        int modifierToApply = GetModifierToApply(unitExecutor);
        target.ApplyStatModifier(Stat, modifierToApply);

        return PrintLog(unitExecutor, modifierToApply);
    }
}