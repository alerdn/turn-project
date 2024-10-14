using UnityEngine;

public class PlayerWaitingState : PlayerBaseState
{

    public PlayerWaitingState(PlayerController stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        stateMachine.Animator.CrossFadeInFixedTime("FreelookBlendTree", .1f);
        stateMachine.Animator.SetFloat("FreelookBlendX", 0f);
        stateMachine.Animator.SetFloat("FreelookBlendY", 0f);
    }

    public override void OnTick(float deltaTime)
    {
    }

    public override void OnExit()
    {
    }
}