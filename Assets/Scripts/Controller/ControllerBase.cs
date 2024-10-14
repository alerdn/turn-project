using UnityEngine;

public abstract class ControllerBase : StateMachine
{
    public Unit Unit => unit;

    [SerializeField] protected Unit unit;
}