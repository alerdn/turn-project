public abstract class BattleStateBase : State
{
    protected BattleManager stateMachine;

    public BattleStateBase(BattleManager stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}