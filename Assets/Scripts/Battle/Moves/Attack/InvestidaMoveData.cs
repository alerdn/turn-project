using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "InvestidaMoveData", menuName = "Moves/Attack/Investida")]
public class InvestidaMoveData : AttackMoveData
{
    public override async Task<string> Execute(Unit unitExecutor)
    {
        InteractionsData.ForEach(interaction => interaction.HasInteracted = false);
        unitExecutor.DecreaseEnergy(EnergyCost);
        target = unitExecutor.Enemy;

        await Task.Delay(Mathf.RoundToInt(MoveDuration * 1000f));

        float damageToApply = GetDamageToApply(unitExecutor);
        target.TakeDamage(damageToApply);

        return PrintLog(unitExecutor, damageToApply);
    }
}