using UnityEngine;

public class WendigoIdleState : WendigoBaseState
{
    public override void EnterState(WendigoStateManager stateManager)
    {
    }

    public override void UpdateState(WendigoStateManager stateManager)
    {
        ChangeState(stateManager);
    }
    
    private static void ChangeState(WendigoStateManager stateManager)
    {
        if (stateManager.IsReadyToComeOut)
        {
            stateManager.SwitchState(stateManager.ComeOutState);
        }
        
        if (stateManager.IsReadyToComeOut && Vector3.Distance(stateManager.transform.position, stateManager.PlayerTrans.position) <= stateManager.AttackRange)
        {
            stateManager.SwitchState(stateManager.AttackState);
        }
    }
}
