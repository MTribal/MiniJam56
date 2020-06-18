public class CookieStateWalk : CookieState
{
    public CookieStateWalk(Cookie cookieStateMachine) : base(cookieStateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        if (stateMachine.ShouldAttack())
        {
            Exit();
        }
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Animator.SetBool("IsAttacking", true);
        stateMachine.SetState(new CookieStateAttack(stateMachine));
    }
}
