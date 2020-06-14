using UnityEngine;

public class DoughnutIdleState : DoughnutState
{
    public DoughnutIdleState(Doughnut stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ResetCanWork();
    }

    public override void LogicUpdate()
    {
        if (stateMachine.CanWork && stateMachine.ShouldWork())
        {
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
