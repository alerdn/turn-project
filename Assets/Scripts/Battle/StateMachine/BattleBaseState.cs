public abstract class BattleBaseState : State
{
    protected BattleStateMachine stateMachine;

    public BattleBaseState(BattleStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
