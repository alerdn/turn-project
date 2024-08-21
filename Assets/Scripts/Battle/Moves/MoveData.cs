using System;
using System.Threading.Tasks;
using UnityEngine;

public enum MoveType
{
    None,
    Physical,
    Especial,
    Status
}

public abstract class MoveData : ScriptableObject
{
    public string Name;
    public MoveType Type;
    protected Unit target;

    public virtual Task Execute(Unit unitExecutor)
    {
        if (!target)
        {
            throw new Exception($"Definir target da habilidade {Name}");
        }

        return Task.CompletedTask;
    }
}