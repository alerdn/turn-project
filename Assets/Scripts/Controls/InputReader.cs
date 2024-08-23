using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event Action JumpEvent;

    public float MovementAxis { get; private set; }

    private Controls _controls;

    private void OnEnable()
    {
        _controls ??= new Controls();
        _controls.Player.SetCallbacks(this);
        EnableInputs();
    }

    public void EnableInputs()
    {
        _controls.Player.Enable();
    }

    public void DisableInputs()
    {
        _controls.Player.Disable();
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
}
