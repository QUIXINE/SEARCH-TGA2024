using UnityEngine;
public class WendigoComeOutState : WendigoBaseState
{
    public override void EnterState(WendigoStateManager stateManager)
    {
        Debug.Log("Enter Come Out State");
        stateManager.TargetTrans = stateManager.ComeOutTrans;
    }

    public override void UpdateState(WendigoStateManager stateManager)
    {
        MoveNRotate(stateManager);
        ChangeState(stateManager);
    }

    private static void MoveNRotate(WendigoStateManager stateManager)
    {
        if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) < 0.1f)
        {
            Debug.Log("Rotate");
            //rotate direction
            Quaternion dir = Quaternion.Euler(0,90,0);
            stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, dir, stateManager.InterpolationRatio);
            if (stateManager.transform.rotation == Quaternion.Euler(0,90,0))
            {
                Debug.Log("Change State");
                //rotate look at player's direction to chase player
                stateManager.SwitchState(stateManager.ChaseState);
            }

        }
        //Move and Rotate
        else if (Vector3.Distance(stateManager.transform.position, stateManager.TargetTrans.position) >= 0.1f)
        {
            Debug.Log("Move");
            //Move
            stateManager.CC.Move(new Vector3(0,0, -0.1f * stateManager.WalkSpeed * Time.deltaTime));
            //rotate direction
            Quaternion dir = Quaternion.Euler(0,180,0);
            stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, dir, stateManager.InterpolationRatio);
        }
    }

    private static void ChangeState(WendigoStateManager stateManager)
    {
        if (Vector3.Distance(stateManager.transform.position, stateManager.PlayerTrans.position) <= stateManager.AttackRange)
        {
            stateManager.SwitchState(stateManager.AttackState);
        }
    }
}
