using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "InvestidaMoveData", menuName = "Moves/Investida")]
public class InvestidaMoveData : AttackMoveData
{
    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor.Enemy;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}