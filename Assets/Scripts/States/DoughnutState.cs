using My_Utils;

public abstract class DoughnutState : State
{
    protected Doughnut stateMachine;

    public DoughnutState(Doughnut stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
