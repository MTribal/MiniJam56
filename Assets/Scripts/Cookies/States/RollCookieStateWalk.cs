public class RollCookieStateWalk : CookieState
{
    public RollCookieStateWalk(Cookie cookieStateMachine) : base(cookieStateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        if (stateMachine.ShouldAttack())
            stateMachine.Attack();
    }
}
