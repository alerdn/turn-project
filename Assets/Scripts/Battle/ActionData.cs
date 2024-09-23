using System.Threading.Tasks;
using UnityEngine;

public abstract class ActionData : ScriptableObject
{
    public string Name;
    public float ActionDuration;

    public abstract Task<string> Execute(Unit unitExecutor);
}