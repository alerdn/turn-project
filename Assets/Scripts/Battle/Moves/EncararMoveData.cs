using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EncararMoveData", menuName = "Moves/Encarar")]
public class EncararMoveData : MoveData
{
    public float DebuffPercentage = -.1f;

    private void OnEnable()
    {
        Name = "Encarar";
        Type = MoveType.Status;
    }

    public override async Task Execute(Unit unitExecutor)
    {
        unitExecutor.Enemy.ApplyAttackModifier(DebuffPercentage);

        Debug.LogWarning($"{unitExecutor.Name} usou {Name} em {unitExecutor.Enemy.Name} e diminuiu seu ataque para {unitExecutor.Enemy.Attack}");
        await Task.Delay(1000);
    }
}