using My_Utils;

public class CookieState : State
{
    protected Cookie stateMachine;

    public CookieState(Cookie cookieStateMachine)
    {
        stateMachine = cookieStateMachine;
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
