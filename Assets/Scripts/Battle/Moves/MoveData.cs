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
    protected Unit target;

    public virtual Task Execute(Unit unitExecutor)
    {
        if (!target)
        {
            throw new Exception($"Definir target da habilidade {Name}");
        }

        unitExecutor.DecreaseEnergy(EnergyCost);
        return Task.CompletedTask;
    }
}