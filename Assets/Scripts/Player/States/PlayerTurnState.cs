using System;
using UnityEngine;

public class PlayerTurnState : PlayerBaseState
{
    private PlayerBaseState _previousState;

    public PlayerTurnState(PlayerController stateMachine, PlayerBaseState previousState) : base(stateMachine)
    {
        _previousState = previousState;
    }

    public override void OnEnter()
    {
        stateMachine.InputReader.EnableOffensiveInputs();

        stateMachine.InputReader.ToggleTurnModeEvent += OnToggleTurnMode;
        stateMachine.NotifyEnterTurnMode();

        stateMachine.Animator.CrossFadeInFixedTime("FreelookBlendTree", .1f);
        stateMachine.Animator.SetFloat("FreelookBlendX", 0f);
        stateMachine.Animator.SetFloat("FreelookBlendY", 0f);
    }

    public override void OnTick(float deltaTime)
    {
    }

    public override void OnExit()
    {
        stateMachine.InputReader.DisableOffensiveInputs();

        stateMachine.InputReader.ToggleTurnModeEvent -= OnToggleTurnMode;

        stateMachine.NofityExitTurnMode();
    }

    private void OnToggleTurnMode()
    {
        stateMachine.SwitchState(_previousState);
    }
}