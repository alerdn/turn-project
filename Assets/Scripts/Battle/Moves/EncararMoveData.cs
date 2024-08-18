using UnityEngine;

[CreateAssetMenu(fileName = "EncararMoveData", menuName = "Moves/Encarar")]
public class EncararMoveData : MoveData
{
    private void OnEnable()
    {
        Name = "Encarar";
        Type = MoveType.Status;
    }

    public override void Execute(Unit unit)
    {
        Debug.Log($"{unit.Name} usou escarar em {unit.Enemy} e diminuiu seu ataque");
        unit.Enemy.ApplyAttackModifier(-.1f);
    }
}