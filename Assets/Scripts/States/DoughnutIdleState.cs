using UnityEngine;

public class DoughnutIdleState : DoughnutState
{
    public DoughnutIdleState(Doughnut stateMachine) : base(stateMachine) { }

    public override void LogicUpdate()
    {
        if (stateMachine.CanWork && stateMachine.ShouldWork())
        {
            stateMachine.ResetCanWork();
            Exit();
        }
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Animator.SetBool("IsWorking", true);
        stateMachine.SetState(new DoughnutWorkState(stateMachine));
    }
}
