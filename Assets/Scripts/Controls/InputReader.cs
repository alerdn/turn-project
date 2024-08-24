using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, IPlayerActions, IBattleActions
{
    public event Action JumpEvent;
    public event Action InteractEvent;

    public float MovementAxis { get; private set; }
    public Controls Controls => _controls;

    private Controls _controls;

    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Player.SetCallbacks(this);
        _controls.Battle.SetCallbacks(this);
        EnablePlayerInputs();
    }

    public void EnablePlayerInputs()
    {
        _controls.Player.Enable();
    }

    public void EnableBattleInputs()
    {
        _controls.Battle.Enable();
    }

    public void DisablePlayerInputs()
    {
        _controls.Player.Disable();
    }

    public void DisableBattleInputs()
    {
        _controls.Battle.Disable();
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        InteractEvent?.Invoke();
    }
}
