using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "InvestidaMoveData", menuName = "Moves/Investida")]
public class InvestidaMoveData : MoveData
{
    public float Damage = .4f;

    private void OnEnable()
    {
        Name = "Investida";
        Type = MoveType.Physical;
    }

    public override async Task Execute(Unit unitExecutor)
    {
        float damageToApplay = unitExecutor.Attack * Damage;
        unitExecutor.Enemy.TakeDamage(damageToApplay);

        Debug.LogError($"{unitExecutor.Name} Usou investida em {unitExecutor.Enemy.Name} e causou {damageToApplay} pontos de dano");
        await Task.Delay(1000);
    }
}