using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "GrunhidoMoveData", menuName = "Moves/Grunhido")]
public class GrunhidoMoveData : StatusMoveData
{
    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor.Enemy;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}