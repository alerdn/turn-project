using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EncararMoveData", menuName = "Moves/Status/Encarar")]
public class EncararMoveData : StatusMoveData
{
    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor.Enemy;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}