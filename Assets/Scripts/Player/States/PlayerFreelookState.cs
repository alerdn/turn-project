using UnityEngine;

public class PlayerFreelookState : PlayerBaseState
{
    public PlayerFreelookState(PlayerController stateMachine) : base(stateMachine) { }

    public override void OnEnter()
    {
        stateMachine.InputReader.EnableMovementInputs();
        stateMachine.Animator.CrossFadeInFixedTime("FreelookBlendTree", .1f);
    }

    public override void OnTick(float deltaTime)
    {
        PlayerMovementComponent movementComponent = stateMachine.MovementComponent;

        float freelookBlendX = 0f;
        float freelookBlendY = 0f;
        if (movementComponent.IsGrounded)
        {
            freelookBlendX = movementComponent.FrameVelocity.normalized.x == 0f ? 0f : 1f;
        }
        else
        {
            freelookBlendY = movementComponent.FrameVelocity.normalized.y == 0f ? 0f : 1f;
        }

        stateMachine.Animator.SetFloat("FreelookBlendX", freelookBlendX);
        stateMachine.Animator.SetFloat("FreelookBlendY", freelookBlendY);
    }

    public override void OnExit()
    {
        stateMachine.InputReader.DisableMovementInputs();
    }
}