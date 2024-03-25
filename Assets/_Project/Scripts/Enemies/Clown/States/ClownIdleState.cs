using UnityEngine;

public class ClownIdleState : ClownBaseState
{
    public override void EnterState(ClownStateManager stateManager)
    {
        
    }

    public override void UpdateState(ClownStateManager stateManager)
    {
        if (stateManager.IsReadyToComeOut || stateManager.IsReadyToComeOutGetBox)
        {
            stateManager.SwitchState(stateManager.ComeOutState);
        }
    }
}