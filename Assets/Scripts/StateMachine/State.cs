public abstract class State
{
    public abstract void OnEnter();

    public abstract void OnTick(float deltaTime);

    public abstract void OnExit();
}