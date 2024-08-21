using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "WebStringMoveData", menuName = "Moves/Web String")]
public class WebStringMoveData : StatusMoveData
{
    private void OnEnable()
    {
        Name = "Fio de Seda";
        Type = MoveType.Status;
        Attribute = UnitAttribute.Speed;
    }

    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor.Enemy;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}