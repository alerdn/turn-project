using System;
using System.Collections.Generic;

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

public abstract class MoveData : ActionData
{
    public MoveType Type;
    public int EnergyCost;
    public EInteractableBy InteractableBy;
    public List<InteractionData> InteractionsData;
    public List<CameraShakeSetting> ShakeSettings;
    protected Unit executor;
    protected Unit target;
}