using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordDanceMoveData", menuName = "Moves/Sword Dance")]
public class SwordDanceMoveData : MoveData
{
    public float BuffPercentage = .1f;

    private void OnEnable()
    {
        Name = "Dança das Espadas";
        Type = MoveType.Status;
    }

    public override async Task Execute(Unit unitExecutor)
    {
        unitExecutor.ApplyAttackModifier(BuffPercentage);

        Debug.LogWarning($"{unitExecutor.Name} usou {Name} em si mesmo e aumentou seu ataque para {unitExecutor.Attack}");
        await Task.Delay(1000);
    }
}