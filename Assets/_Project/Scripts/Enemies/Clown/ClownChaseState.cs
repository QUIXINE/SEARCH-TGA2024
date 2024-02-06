using UnityEngine;
public class ClownChaseState : ClownBaseState
{
    public override void EnterState(ClownStateManager clownStateManager)
    {
    }

    public override void UpdateState(ClownStateManager stateManager)
    {
        Vector3 m_lookPos;
        m_lookPos = stateManager.PlayerTransform.position - stateManager.transform.position;
        m_lookPos.y = 0;
        /*Set transform of effector of clown head
         Quaternion rotation = Quaternion.LookRotation(m_lookPos);
        stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.ChaseSpeed * Time.deltaTime);*/

        //m_animator.SetBool("Chase", true);
        stateManager.transform.rotation = Quaternion.Euler(0,0,0);
        stateManager.ClownNavMeshAgent.SetDestination(stateManager.PlayerTransform.position);

        /*if (Vector3.Distance(stateManager.transform.position, target.position) <= AttackRange)
        {
            m_animator.SetBool("Chase", false);
            stateManager.SwitchState(stateManager.AttackState);
        }*/
    }
}