public abstract class PlayerBaseState : State
{
    protected PlayerController stateMachine;

    protected PlayerBaseState(PlayerController stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}