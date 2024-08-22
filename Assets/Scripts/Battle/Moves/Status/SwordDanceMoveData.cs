using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordDanceMoveData", menuName = "Moves/Status/Sword Dance")]
public class SwordDanceMoveData : StatusMoveData
{
    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}