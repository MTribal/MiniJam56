public class DoughnutWorkState : DoughnutState
{
    public DoughnutWorkState(Doughnut stateMachine) : base(stateMachine)
    {
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.Animator.SetBool("IsWorking", false);
        stateMachine.SetState(new DoughnutIdleState(stateMachine));
    }
}
