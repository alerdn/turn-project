using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    public Unit Unit => unit;

    [SerializeField] protected Unit unit;
}