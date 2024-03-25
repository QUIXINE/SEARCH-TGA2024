using UnityEngine;

public class WendigoAttackState : WendigoBaseState
{
    public override void EnterState(WendigoStateManager stateManager)
    {
        Debug.Log("Enter Attack State");
        // if time of waitforseceond of Repawn() in PlayerRespawn.cs is not suitable(not enough to wait for eating animation),
        // stop PlayerController, check if animation is finish to call player.TakeDamage()
        // player.TakeDamage()
        var player = stateManager.PlayerTrans.gameObject.GetComponent<PlayerTakeDamage>();
        player.TakeDamage(stateManager.gameObject.scene);
        stateManager.PlayerTrans.parent = stateManager.HoldingHandTrans;
        // play animation attack stateManager.MyAnimator
        stateManager.MyAnimator.SetBool(stateManager.AttackAnimId, true);
    }

    public override void UpdateState(WendigoStateManager stateManager)
    {
        if (stateManager.PlayerTrans.parent == stateManager.HoldingHandTrans)
        {
            stateManager.PlayerTrans.position = new Vector3(stateManager.HoldingHandTrans.position.x, 
                stateManager.HoldingHandTrans.position.y + stateManager.PlayerYAxisHolded, stateManager.HoldingHandTrans.position.z);
            stateManager.PlayerTrans.rotation = Quaternion.LookRotation(stateManager.HeadTrans.position);
        }
        
        //Problem : After Wendigo catches player, player will be in HoldingHandTrans as a child.
        //By that reason, it makes player be in scene of Wendigo which means if that scene is reloaded, player will be reloaded as the obj of that scene or put it simply, player will be moved out of the game
        //Try if animation finishes then makes player's parent is obj of Player scene or try null and see if player obj will move back to its owb scene
        /*if (stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && stateManager.MyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 
            && !stateManager.MyAnimator.IsInTransition(0))
        {
            stateManager.PlayerTrans.parent =
                stateManager.PlayerTrans.GetComponent<PlayerRespawn>().MyParent;
        }*/
    }
}
