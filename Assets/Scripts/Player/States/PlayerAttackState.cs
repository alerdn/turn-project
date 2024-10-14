using System;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private float _time;
    private PlayerBaseState _previousState;
    private Vector3 _originalPosition;

    public PlayerAttackState(PlayerController stateMachine, Vector2 originalPosition, PlayerBaseState previousState) : base(stateMachine)
    {
        _time = 0f;
        _previousState = previousState;
        _originalPosition = originalPosition;
    }

    public override void OnEnter()
    {
        stateMachine.Animator.CrossFadeInFixedTime("Attack", 0.1f);
        stateMachine.CombatComponent.ApplyDamage(stateMachine.Unit.Enemy, MoveType.Physical);
        stateMachine.Unit.ChargeEnergy(.25f);
    }

    public override void OnTick(float deltaTime)
    {
        Vector2 enemyPosition = stateMachine.Unit.Enemy.transform.position;
        stateMachine.transform.position = new Vector2(enemyPosition.x - 3f, enemyPosition.y);

        _time += deltaTime;

        if (_time > .4f)
        {
            stateMachine.transform.position = _originalPosition;
            stateMachine.SwitchState(_previousState);
        }
    }

    public override void OnExit()
    {
    }
}