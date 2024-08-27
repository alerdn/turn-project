using UnityEngine;

public class Gun : MonoBehaviour
{
    public AttackMoveData AttackMove => _attackMove;

    [SerializeField] private AttackMoveData _attackMove;
}