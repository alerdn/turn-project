using System;
using System.Threading.Tasks;
using UnityEngine;

public enum MoveType
{
    None,
    Physical,
    Special,
    Status
}

public abstract class MoveData : ScriptableObject
{
    public string Name;
    public MoveType Type;
    public int EnergyCost;
    public float MoveDuration;
    public float InteractionWindowTime;
    public float InteractionWindowDuration = .5f;
    public bool HasInteracted;
    protected Unit target;

    public abstract Task Execute(Unit unitExecutor);
}