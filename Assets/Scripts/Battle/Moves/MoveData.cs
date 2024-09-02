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

[Flags]
public enum EInteractableBy
{
    None = 0,
    Player = 1 << 0,
    Enemy = 1 << 1
}

public abstract class MoveData : ScriptableObject
{
    public string Name;
    public MoveType Type;
    public int EnergyCost;
    public float MoveDuration;
    public EInteractableBy InteractableBy;
    public List<InteractionData> InteractionsData;
    public List<CameraShakeSetting> ShakeSettings;
    public MoveEffect EffectPrefab;
    protected Unit executor;
    protected Unit target;

    public abstract Task<string> Execute(Unit unitExecutor);
}