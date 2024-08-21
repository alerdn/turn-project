using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EncararMoveData", menuName = "Moves/Encarar")]
public class EncararMoveData : StatusMoveData
{
    private void OnEnable()
    {
        Name = "Encarar";
        Type = MoveType.Status;
        Attribute = UnitAttribute.Attack;
    }

    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor.Enemy;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}