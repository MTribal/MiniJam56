using My_Utils;

public abstract class DoughnutState : State
{
    protected Doughnut stateMachine;

    public DoughnutState(Doughnut stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public override void Enter()
    {
        if (!GameManager.Instance.IsPlaying) return;
    }

    public override void HandleInput()
    {
        if (!GameManager.Instance.IsPlaying) return;

        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        if (!GameManager.Instance.IsPlaying) return;

        base.PhysicsUpdate();
    }

    public override void Exit()
    {
        if (!GameManager.Instance.IsPlaying) return;

        base.Exit();
    }
}
