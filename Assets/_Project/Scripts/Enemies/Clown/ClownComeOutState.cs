public class ClownComeOutState : ClownBaseState
{
    public override void EnterState(ClownStateManager stateManager)
    {
        stateManager.SwitchState(stateManager.ChaseState);
    }

    public override void UpdateState(ClownStateManager clownStateManager)
    {
    }
}
