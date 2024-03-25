using UnityEngine;
public class ClownChaseState : ClownBaseState
{
    public override void EnterState(ClownStateManager stateManager)
    {
        // stateManager.HoldTrans.chi
    }

    public override void UpdateState(ClownStateManager stateManager)
    {
        Vector3 m_lookPos;
        m_lookPos = stateManager.PlayerTrans.position - stateManager.transform.position;
        m_lookPos.y = 0;
        /*Set transform of effector of clown head
         Quaternion rotation = Quaternion.LookRotation(m_lookPos);
        stateManager.transform.rotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.ChaseSpeed * Time.deltaTime);*/

        //m_animator.SetBool("Chase", true);
        Vector3 offset = stateManager.PlayerTrans.position - stateManager.transform.position;
        float range = offset.sqrMagnitude;
        // Debug.Log("range: " + range);
        Vector3 dirPlayerToThis = (stateManager.PlayerTrans.position - stateManager.transform.position).normalized;
        float dot = Vector3.Dot(stateManager.transform.right, dirPlayerToThis);
        // Debug.Log("dot: " + dot);

        // Debug.Log("dir: " + dot);
        
        Rotate(stateManager);

        if (range < stateManager.ChaseRange && range > stateManager.AttackRange)
        {
            if (stateManager.gameObject.name == "Clown In House")
            {
                stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position, new Vector3(
                    stateManager.PlayerTrans.position.x, stateManager.transform.position.y,
                    stateManager.PlayerTrans.position.z), stateManager.ChaseSpeed * Time.deltaTime);
            }
            else if (stateManager.gameObject.name == "Clown In Underground")
            {
                Debug.Log("Move");
                Debug.Log(dot);
                stateManager.MyAnimator.SetBool(stateManager.IsIdleAnimId, false);
                stateManager.MyAnimator.SetBool(stateManager.IsWalkingAnimId, true);
                stateManager.transform.Translate(new Vector3(0,0,(dot > 0? -2 : 2) * stateManager.ChaseSpeed * Time.deltaTime));
            }
        }
        else if (range <= stateManager.AttackRange)
        {
            // stateManager.animator.SetBool("Chase", false);
            stateManager.SwitchState(stateManager.AttackState);
        }
    }

    private static void Rotate(ClownStateManager stateManager)
    {
        Vector3 dirPlayerToThis = (stateManager.PlayerTrans.position - stateManager.transform.position);
        float angle = Vector3.Angle(stateManager.transform.right, dirPlayerToThis);
        //Rotate transform rotation
        //if use dot clown will rotate to 0 and back to 180 vice versa because when the clown rotates to middle of the way
        //dot will change back and forth between 0 and -0.0....
        Quaternion rotation = Quaternion.Euler(0,0,0); // --> this maybe only available for puzzle 6 because puzzle 1 clown has to rotate to player's dir
        if (Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270)
        {
            Debug.DrawLine(stateManager.transform.position, stateManager.PlayerTrans.position, Color.green);
            rotation = Quaternion.Euler(0,180,0);

        }

        if (stateManager.gameObject.name == "Clown In Underground")
        {
            Debug.Log("Rotate");
            rotation = Quaternion.Euler(0,270,0);
            stateManager.transform.localRotation = Quaternion.Slerp(stateManager.transform.rotation, rotation, stateManager.RotationSpeed * Time.deltaTime);
        }
        else if (stateManager.gameObject.name == "Clown In House")
        {
            Debug.Log("Rotate");
            Vector3 playerPos =
                new Vector3(stateManager.PlayerTrans.position.x, stateManager.transform.position.y, stateManager.PlayerTrans.position.z);
            Vector3 clownPos =
                new Vector3(stateManager.transform.position.x, 0, stateManager.transform.position.z);
            //dirPlayerToThis = playerPos - clownPos;
            rotation = Quaternion.LookRotation(dirPlayerToThis);
            Quaternion q = Quaternion.Inverse(rotation);
            dirPlayerToThis = (stateManager.transform.position - stateManager.PlayerTrans.position);

            stateManager.transform.rotation = Quaternion.LookRotation(dirPlayerToThis /*new Vector3(dirPlayerToThis.x, 0, dirPlayerToThis.z)*/);
            //stateManager.transform.rotation = Quaternion.LookRotation(new Vector3(stateManager.PlayerTrans.position.x, stateManager.transform.position.y, stateManager.PlayerTrans.position.z));
        }
    }
}