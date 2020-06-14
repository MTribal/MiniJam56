using UnityEngine;

public class CookieStateAttack : CookieState
{
    public CookieStateAttack(Cookie cookieStateMachine) : base(cookieStateMachine)
    {  
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Animator.SetBool("IsAttacking", false);
        stateMachine.SetState(new CookieStateIdle(stateMachine));
    }
}
