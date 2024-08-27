using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum MoveType
{
    None,
    Physical,
    Special,
    Status
}

[Serializable]
public class InteractionData
{
    public float AntecipationTime;
    public float InteractionWindowTime;
    public float InteractionWindowDuration;
    public bool HasInteracted;
}

public abstract class MoveData : ScriptableObject
{
    public string Name;
    public MoveType Type;
    public int EnergyCost;
    public float MoveDuration;
    public List<InteractionData> InteractionsData;
    protected Unit target;

    public abstract Task Execute(Unit unitExecutor);
}