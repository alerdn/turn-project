using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordDanceMoveData", menuName = "Moves/Sword Dance")]
public class SwordDanceMoveData : StatusMoveData
{
    private void OnEnable()
    {
        Name = "Dança das Espadas";
        Type = MoveType.Status;
        Attribute = UnitAttribute.Attack;
    }

    public override async Task Execute(Unit unitExecutor)
    {
        target = unitExecutor;
        await base.Execute(unitExecutor);
        await Task.Delay(1000);
    }
}