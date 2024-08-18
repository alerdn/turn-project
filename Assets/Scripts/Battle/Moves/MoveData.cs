using UnityEngine;

public enum MoveType
{
    Physical,
    Especial,
    Status
}

public abstract class MoveData : ScriptableObject
{
    public string Name;
    public MoveType Type;

    public abstract void Execute(Unit unit);
}