using UnityEngine;

public class PlayerBattleState : PlayerBaseState
{
    public PlayerBattleState(PlayerController stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        stateMachine.InputReader.EnableOffensiveInputs();

        stateMachine.InputReader.AttackEvent += OnAttack;
        stateMachine.InputReader.ToggleTurnModeEvent += OnToggleTurnMode;

        stateMachine.Animator.CrossFadeInFixedTime("FreelookBlendTree", .1f);
        stateMachine.Animator.SetFloat("FreelookBlendX", 0f);
        stateMachine.Animator.SetFloat("FreelookBlendY", 0f);
    }

    public override void OnTick(float deltaTime)
    {
    }

    public override void OnExit()
    {
        stateMachine.InputReader.AttackEvent -= OnAttack;
        stateMachine.InputReader.ToggleTurnModeEvent -= OnToggleTurnMode;

        stateMachine.InputReader.DisableOffensiveInputs();
    }

    private void OnAttack()
    {
        stateMachine.SwitchState(new PlayerAttackState(stateMachine, stateMachine.transform.position, this));
    }

    private void OnToggleTurnMode()
    {
        stateMachine.SwitchState(new PlayerTurnState(stateMachine, this));
    }
}