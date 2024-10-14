using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, IFreelookActions, IOffensiveActions, IDefensiveActions
{
    public event Action JumpEvent;
    public event Action AttackEvent;
    public event Action ToggleTurnModeEvent;
    public event Action InteractEvent;

    public bool IsMovementInputsEnabled => _controls.Freelook.enabled;
    public bool IsOffensiveInputsEnabled => _controls.Offensive.enabled;
    public bool IsDefensiveInputsEnabled => _controls.Defensive.enabled;
    public float MovementAxis { get; private set; }
    public Controls Controls => _controls;

    private Controls _controls;

    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Freelook.SetCallbacks(this);
        _controls.Offensive.SetCallbacks(this);
        _controls.Defensive.SetCallbacks(this);
    }

    #region Movement

    public void EnableMovementInputs()
    {
        _controls.Freelook.Enable();
    }

    public void DisableMovementInputs()
    {
        _controls.Freelook.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementAxis = context.ReadValue<float>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        JumpEvent?.Invoke();
    }

    #endregion

    #region Offensive

    public void EnableOffensiveInputs()
    {
        _controls.Offensive.Enable();
    }

    public void DisableOffensiveInputs()
    {
        _controls.Offensive.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        AttackEvent?.Invoke();
    }

    public void OnToggleTurnMode(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        ToggleTurnModeEvent?.Invoke();
    }

    #endregion

    #region Defensive

    public void EnableDefensiveInputs()
    {
        _controls.Defensive.Enable();
    }

    public void DisableDefensiveInputs()
    {
        _controls.Defensive.Disable();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        InteractEvent?.Invoke();
    }

    #endregion
}
