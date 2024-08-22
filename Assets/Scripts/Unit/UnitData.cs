using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "Unit")]
public class UnitData : ScriptableObject
{
    public string Name;
    public float Health;
    public float Attack;
    public float SpecialAttack;
    public float Defence;
    public float SpecialDefence;
    public float Speed;
    public List<MoveData> Moves;
}