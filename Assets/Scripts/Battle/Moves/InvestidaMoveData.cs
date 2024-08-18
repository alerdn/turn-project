using UnityEngine;

[CreateAssetMenu(fileName = "InvestidaMoveData", menuName = "Moves/Investida")]
public class InvestidaMoveData : MoveData
{
    private void OnEnable()
    {
        Name = "Investida";
        Type = MoveType.Physical;
    }

    public override void Execute(Unit unit)
    {
        Debug.Log($"{unit.Name} Usou investida em {unit.Enemy} e causou 10 pontos de dano");
        unit.Enemy.TakeDamage(10f);
    }
}