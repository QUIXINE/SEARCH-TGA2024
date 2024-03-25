using UnityEngine;

public class ClownAttackState : ClownBaseState
{
    public override void EnterState(ClownStateManager stateManager)
    {
        Debug.Log("Enter Attack State");
        stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, false);
        stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, true);
        var takeDamage = stateManager.PlayerTrans.gameObject.GetComponent<PlayerTakeDamage>();
        takeDamage.TakeDamage(stateManager.gameObject.scene);
    }

    public override void UpdateState(ClownStateManager stateManager)
    {
        //Try animation curve to lerp from stand animation to sit and press player's head
        
    }
}
