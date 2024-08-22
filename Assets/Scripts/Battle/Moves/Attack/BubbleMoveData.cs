using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleMoveData", menuName = "Moves/Attack/Bubble")]
public class BubbleMoveData : AttackMoveData
{
    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor.Enemy;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}