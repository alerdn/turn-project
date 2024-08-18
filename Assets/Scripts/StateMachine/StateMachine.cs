using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State _currentState;

    private void Update()
    {
        _currentState?.OnTick(Time.deltaTime);
    }

    public void SwitchState(State newState)
    {
        _currentState?.OnExit();
        _currentState = newState;
        _currentState?.OnEnter();
    }
}